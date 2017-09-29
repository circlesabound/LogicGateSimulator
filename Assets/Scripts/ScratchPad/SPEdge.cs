using System;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;

namespace Assets.Scripts.ScratchPad
{
    public class SPEdge : MonoBehaviour, IPointerClickHandler
    {
        private SPConnector AnchorConnector;
        private SPCanvas Canvas;
        private bool Finalised;
        private Connection BackendConnection;

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
            }

            if (connector.ConnectorType == SPConnectorType.SPInConnector)
            {
                // InConnectors should only have one incoming edge
                if (InConnector == null && connector.ConnectedEdges.Count == 0)
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
                if (OutConnector == null)
                {
                    OutConnector = (SPOutConnector)connector;
                }
                else
                {
                    throw new ArgumentException();
                }
            }
        }

        public void Finalise()
        {
            if (!Finalised && InConnector != null && OutConnector != null && AnchorConnector != null)
            {
                // finalise line position
                LineRenderer.SetPosition(0, OutConnector.gameObject.transform.position);
                LineRenderer.SetPosition(1, InConnector.gameObject.transform.position);

                // register edge with connector endpoints
                OutConnector.ConnectedEdges.Add(this);
                InConnector.ConnectedEdges.Add(this);
                SPLogicComponent outConnectorComponent = OutConnector.ParentComponent;
                SPLogicComponent inConnectorComponent = InConnector.ParentComponent;
                Assert.IsNotNull(outConnectorComponent);
                Assert.IsNotNull(inConnectorComponent);

                // update canvas and backend
                Canvas.Edges.Add(this.gameObject);
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

        public void Delete()
        {
            if (Finalised)
            {
                // Update canvas and backend
                Canvas.Circuit.RemoveComponent(BackendConnection);
                Canvas.Edges.Remove(this.gameObject);

                // Unregister edge with connector endpoints
                if (OutConnector != null) OutConnector.ConnectedEdges.Remove(this);
                if (InConnector != null) InConnector.ConnectedEdges.Remove(this);
            }

            // Destroy GameObject
            Destroy(this.gameObject);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            Debug.Log("Edge| " + Canvas.CurrentTool.ToString() + " | " + eventData.button.ToString());
        }

        protected void Awake()
        {
            Finalised = false;
        }

        // Update is called once per frame
        protected void Update()
        {
            if (!Finalised)
            {
                var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                LineRenderer.SetPosition(1, mousePos);
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

        private void Start()
        {
            Canvas = FindObjectOfType<SPCanvas>();
            Assert.IsNotNull(Canvas);
            LineRenderer = GetComponent<LineRenderer>();
            Assert.IsNotNull(LineRenderer);
            Assert.IsNotNull(AnchorConnector);
            LineRenderer.SetPosition(0, AnchorConnector.gameObject.transform.position);
            LineRenderer.SetPosition(1, AnchorConnector.gameObject.transform.position);
        }
    }
}
