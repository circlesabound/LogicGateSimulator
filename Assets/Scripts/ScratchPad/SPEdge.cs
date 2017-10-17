using Assets.Scripts.Savefile;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;

namespace Assets.Scripts.ScratchPad
{
    public class SPEdge : MonoBehaviour, IPointerClickHandler
    {
        private SPConnector AnchorConnector;
        private Connection BackendConnection;
        private SPCanvas Canvas;
        private bool Finalised;
        private LineRenderer LineRenderer;

        public SPInConnector InConnector
        {
            get;
            private set;
        }

        public SPOutConnector OutConnector
        {
            get;
            private set;
        }

        public void AddConnector(SPConnector connector)
        {
            if (Finalised) throw new InvalidOperationException();
            if (AnchorConnector == null)
            {
                AnchorConnector = connector;
                LineRenderer.SetPosition(0, AnchorConnector.gameObject.transform.position);
                LineRenderer.SetPosition(1, AnchorConnector.gameObject.transform.position);
            }

            if (connector.ConnectorType == SPConnectorType.SPInConnector)
            {
                if (InConnector == null && connector.ConnectedEdge == null)
                {
                    InConnector = (SPInConnector)connector;
                }
                else
                {
                    throw new ArgumentException();
                }
            }
            else if (connector.ConnectorType == SPConnectorType.SPOutConnector)
            {
                if (OutConnector == null && connector.ConnectedEdge == null)
                {
                    OutConnector = (SPOutConnector)connector;
                }
                else
                {
                    throw new ArgumentException();
                }
            }
        }

        public void Delete()
        {
            if (Finalised)
            {
                // Update canvas and backend
                Canvas.Circuit.RemoveComponent(BackendConnection);
                Canvas.Edges.Remove(this);

                // Unregister edge with connector endpoints
                if (OutConnector != null) OutConnector.ConnectedEdge = null;
                if (InConnector != null) InConnector.ConnectedEdge = null;
            }

            // Destroy GameObject
            Destroy(this.gameObject);
        }

        public void Finalise()
        {
            if (!Finalised && InConnector != null && OutConnector != null && AnchorConnector != null)
            {
                // update line position
                UpdatePosition();

                // register edge with connector endpoints
                OutConnector.ConnectedEdge = this;
                InConnector.ConnectedEdge = this;
                SPLogicComponent outConnectorComponent = OutConnector.ParentComponent;
                SPLogicComponent inConnectorComponent = InConnector.ParentComponent;
                Assert.IsNotNull(outConnectorComponent);
                Assert.IsNotNull(inConnectorComponent);

                // update canvas and backend
                Canvas.Edges.Add(this);
                BackendConnection = new Connection();
                Canvas.Circuit.AddComponent(BackendConnection);
                Canvas.Circuit.Connect(
                    outConnectorComponent.LogicComponent, OutConnector.ConnectorId,
                    BackendConnection, 0);
                Canvas.Circuit.Connect(
                    BackendConnection, 0,
                    inConnectorComponent.LogicComponent, InConnector.ConnectorId);

                Finalised = true;
            }
            else
            {
                throw new InvalidOperationException();
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            Debug.Log("Edge| " + Canvas.CurrentTool.ToString() + " | " + eventData.button.ToString());
        }

        public void UpdatePosition()
        {
            Assert.IsNotNull(InConnector);
            Assert.IsNotNull(OutConnector);
            LineRenderer.SetPosition(0, OutConnector.gameObject.transform.position);
            LineRenderer.SetPosition(1, InConnector.gameObject.transform.position);
        }

        protected void Awake()
        {
            Finalised = false;
            LineRenderer = GetComponent<LineRenderer>();
            Assert.IsNotNull(LineRenderer);
            Canvas = FindObjectOfType<SPCanvas>();
            Assert.IsNotNull(Canvas);
        }

        // Update is called once per frame
        protected void Update()
        {
            if (!Finalised)
            {
                LineRenderer.SetPosition(1, Util.Util.MouseWorldCoordinates);
            }
            else
            {
                Assert.IsNotNull(BackendConnection);
                if (BackendConnection.Outputs[0] == true)
                {
                    LineRenderer.startColor = Color.green;
                    LineRenderer.endColor = Color.green;
                }
                else
                {
                    LineRenderer.startColor = Color.red;
                    LineRenderer.endColor = Color.red;
                }
            }
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
    }
}
