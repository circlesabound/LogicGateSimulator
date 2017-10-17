using Assets.Scripts.Savefile;
using Assets.Scripts.Util;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;

namespace Assets.Scripts.ScratchPad
{
    public class SPEdge : MonoBehaviour
    {
        private const float LINE_WIDTH = 0.03f;

        private SPConnector AnchorConnector;

        private Connection BackendConnection;
        private SPCanvas Canvas;
        private bool Hover;
        private bool Visible;

        /// <summary>
        /// The SPInConnector that this edge feeds out to.
        /// </summary>
        public SPInConnector InConnector
        {
            get;
            private set;
        }

        /// <summary>
        /// The SPOutConnector feeding into this edge;
        /// </summary>
        public SPOutConnector OutConnector
        {
            get;
            private set;
        }

        private bool Finalised
        {
            get
            {
                return InConnector != null && OutConnector != null;
            }
        }

        private GameObject SegmentLeft
        {
            get
            {
                return gameObject.FindChildGameObject("SPEdgeSegmentLeft");
            }
        }

        private GameObject SegmentMiddle
        {
            get
            {
                return gameObject.FindChildGameObject("SPEdgeSegmentMiddle");
            }
        }

        private GameObject SegmentRight
        {
            get
            {
                return gameObject.FindChildGameObject("SPEdgeSegmentRight");
            }
        }

        public void AddFinishingConnector(SPConnector connector)
        {
            Assert.IsFalse(Finalised);
            Assert.IsNotNull(AnchorConnector);

            Assert.IsNotNull(connector);

            if (AnchorConnector.ConnectorType == connector.ConnectorType)
            {
                // Can't join connectors of the same type (must be in-to-out or out-to-in)
                throw new ArgumentException();
            }
            if (connector.ConnectedEdge != null)
            {
                throw new ArgumentException();
            }

            if (AnchorConnector.ConnectorType == SPConnectorType.SPInConnector)
            {
                OutConnector = (SPOutConnector)connector;
                InConnector = (SPInConnector)AnchorConnector;
            }
            else
            {
                OutConnector = (SPOutConnector)AnchorConnector;
                InConnector = (SPInConnector)connector;
            }

            // Register edge with connector endpoints
            OutConnector.ConnectedEdge = this;
            InConnector.ConnectedEdge = this;

            var outConnectorComponent = OutConnector.ParentComponent;
            var inConnectorComponent = InConnector.ParentComponent;
            Assert.IsNotNull(outConnectorComponent);
            Assert.IsNotNull(inConnectorComponent);

            // Update backend
            BackendConnection = new Connection();
            Canvas.Circuit.AddComponent(BackendConnection);
            Canvas.Circuit.Connect(
                outConnectorComponent.LogicComponent, OutConnector.ConnectorId,
                BackendConnection, 0);
            Canvas.Circuit.Connect(
                BackendConnection, 0,
                inConnectorComponent.LogicComponent, InConnector.ConnectorId);

            // Update canvas
            Canvas.Edges.Add(this);

            AnchorConnector = null;
        }

        public void AddStartingConnector(SPConnector connector)
        {
            Assert.IsFalse(Finalised);
            Assert.IsNull(AnchorConnector);

            Assert.IsNotNull(connector);

            AnchorConnector = connector;
        }

        public void Delete()
        {
            if (Finalised)
            {
                // Update canvas and backend
                Canvas.Circuit.RemoveComponent(BackendConnection);
                Canvas.Edges.Remove(this);

                // Unregister edge with connector endpoints
                OutConnector.ConnectedEdge = null;
                InConnector.ConnectedEdge = null;
            }
            // Destroy gameobject
            Destroy(this.gameObject);
        }

        /// <summary>
        /// Generate a config for this edge.
        /// </summary>
        /// <param name="componentGuidMap">A one-to-one mapping from SPLogicComponents to GUIDs.</param>
        /// <returns>An EdgeConfig which can be used to rebuild this edge.</returns>
        /// <returns></returns>
        public EdgeConfig GenerateConfig(IReadOnlyDictionary<SPLogicComponent, Guid> componentGuidMap)
        {
            return new EdgeConfig(
                componentGuidMap[InConnector.ParentComponent],
                InConnector.ConnectorId,
                componentGuidMap[OutConnector.ParentComponent],
                OutConnector.ConnectorId);
        }

        /// <summary>
        /// Linked to each of the three segments using Unity inspector
        /// </summary>
        public void OnPointerClick(BaseEventData eventData)
        {
            var pointerEventData = (PointerEventData)eventData;
            if (pointerEventData.button == PointerEventData.InputButton.Right)
            {
                if (!Finalised)
                {
                    // Cancel drawing edge
                    Assert.AreEqual(this, Canvas.CurrentEdge);
                    Canvas.CurrentEdge.Delete();
                    Canvas.RestorePreviousTool();
                }
                else
                {
                    // Delete edge
                    Delete();
                }
            }
        }

        /// <summary>
        /// Linked to each of the three segments using Unity inspector
        /// </summary>
        /// <param name="eventData"></param>
        public void OnPointerEnter(BaseEventData eventData)
        {
            Hover = true;
        }

        /// <summary>
        /// Linked to each of the three segments using Unity inspector
        /// </summary>
        /// <param name="eventData"></param>
        public void OnPointerExit(BaseEventData eventData)
        {
            Hover = false;
        }

        private void ApplyToAllSegments(Action<GameObject> action)
        {
            action(SegmentLeft);
            action(SegmentMiddle);
            action(SegmentRight);
        }

        private void Awake()
        {
            Canvas = FindObjectOfType<SPCanvas>();
            Assert.IsNotNull(Canvas);

            // To avoid rendering errors, hide until the first update
            gameObject.GetComponentsInChildren<Renderer>().ForEach(r => r.enabled = false);
        }

        /// <summary>
        /// Scale the edge container while maintaining the line thickness
        /// of the individual line segments
        /// </summary>
        /// <param name="xscale">The amount to scale by in the x direction.</param>
        /// <param name="yscale">The amount to scale by in the y direction.</param>
        private void SetScale(float xscale, float yscale)
        {
            // Make backwards lines look better
            gameObject.transform.localRotation = Quaternion.identity;
            if (xscale < 0)
            {
                var temp = -xscale;
                xscale = yscale;
                yscale = temp;
                gameObject.transform.Rotate(0, 0, 90);
            }

            // Some global scaling shenanigans
            gameObject.transform.localScale = new Vector3
            {
                x = 1,
                y = 1,
                z = gameObject.transform.localScale.z
            };

            float xscaled = xscale / gameObject.transform.lossyScale.x;
            float yscaled = yscale / gameObject.transform.lossyScale.y;
            if (float.IsInfinity(xscaled)) xscaled = 0;
            if (float.IsInfinity(yscaled)) yscaled = 0;

            gameObject.transform.localScale = new Vector3
            {
                x = xscaled,
                y = yscaled,
                z = gameObject.transform.localScale.z
            };

            // Perform inverse scaling along the right axis for each segment
            SegmentLeft.transform.localScale = new Vector3
            {
                x = SegmentLeft.transform.localScale.x,
                y = yscaled != 0 ? LINE_WIDTH / yscaled : 0,
                z = SegmentLeft.transform.localScale.z
            };
            SegmentMiddle.transform.localScale = new Vector3
            {
                x = xscaled != 0 ? LINE_WIDTH / xscaled : 0,
                y = SegmentMiddle.transform.localScale.y,
                z = SegmentMiddle.transform.localScale.z
            };
            SegmentRight.transform.localScale = new Vector3
            {
                x = SegmentRight.transform.localScale.x,
                y = yscaled != 0 ? LINE_WIDTH / yscaled : 0,
                z = SegmentRight.transform.localScale.z
            };

            // Unity complains if the individual segments have a negative scale
            // Nothing bad is happening, but I can't even suppress the warning
        }

        private void Update()
        {
            UpdatePosition();
            Color colour;
            if (Finalised)
            {
                if (Hover)
                {
                    colour = Color.cyan;
                }
                else if (BackendConnection.Outputs[0] == true)
                {
                    colour = Color.green;
                }
                else // if (BackendConnection.Outputs[0] == false)
                {
                    colour = Color.red;
                }
            }
            else
            {
                colour = Color.white;
            }
            ApplyToAllSegments(segment =>
            {
                segment.GetComponent<Renderer>().material.color = colour;
            });
            if (!Visible)
            {
                Visible = true;
                gameObject.GetComponentsInChildren<Renderer>().ForEach(r => r.enabled = true);
            }
        }

        private void UpdatePosition()
        {
            // Calculate the midpoint
            Vector2 newAnchorPosition = Finalised
                ? (Vector2)OutConnector.transform.position
                : (Vector2)AnchorConnector.transform.position;
            Vector2 newAntiAnchorPosition = Finalised
                ? (Vector2)InConnector.transform.position
                : Util.Util.MouseWorldCoordinates;
            Vector2 midpoint = (newAnchorPosition + newAntiAnchorPosition) / 2;

            // Reposition edge container to the new midpoint
            gameObject.transform.position = new Vector3
            {
                x = midpoint.x,
                y = midpoint.y,
                z = gameObject.transform.position.z
            };

            // Determine required scale in both x and y directions
            float newWidth = newAntiAnchorPosition.x - newAnchorPosition.x;
            float newHeight = newAntiAnchorPosition.y - newAnchorPosition.y;
            SetScale(newWidth, newHeight);
        }
    }
}
