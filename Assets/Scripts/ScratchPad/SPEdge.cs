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
                if (InConnector == null)
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
                Finalised = true;
                LineRenderer.SetPosition(0, OutConnector.gameObject.transform.position);
                LineRenderer.SetPosition(1, InConnector.gameObject.transform.position);
                //TODO interface with backend
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
        }

        private void Start()
        {
            Canvas = FindObjectOfType<SPCanvas>();
            Assert.IsNotNull(Canvas);
            LineRenderer = GetComponent<LineRenderer>();
            Assert.IsNotNull(LineRenderer);
            Assert.IsNotNull(AnchorConnector);
            LineRenderer.SetPosition(0, AnchorConnector.gameObject.transform.position);
        }
    }
}
