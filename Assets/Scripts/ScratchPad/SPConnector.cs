using System;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;

namespace Assets.Scripts.ScratchPad
{
    public enum SPConnectorType
    {
        SPInConnector,
        SPOutConnector
    }

    public abstract class SPConnector : MonoBehaviour, IPointerClickHandler
    {
        private SPEdge _ConnectedEdge;
        private SPCanvas Canvas;

        public SPEdge ConnectedEdge
        {
            get
            {
                return _ConnectedEdge;
            }
            set
            {
                if ((_ConnectedEdge == null) == (value == null))
                {
                    throw new InvalidOperationException();
                }
                _ConnectedEdge = value;
            }
        }

        public int ConnectorId
        {
            get;
            private set;
        }

        public SPLogicComponent ParentComponent
        {
            get;
            private set;
        }

        internal abstract SPConnectorType ConnectorType
        {
            get;
        }

        public virtual void OnPointerClick(PointerEventData eventData)
        {
            Debug.Log(ConnectorType.ToString() + "| " + Canvas.CurrentTool.ToString() + " | " + eventData.button.ToString() + " click");
            // if somehow there ends up being more than two connector types
            // it might be better to refactor this functionality into the sub classes
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                if (Canvas.CurrentTool == SPTool.Pointer)
                {
                    if (this.ConnectedEdge == null)
                    {
                        // We're starting a new edge
                        Canvas.StartEdge(this);
                        Canvas.CurrentTool = SPTool.DrawEdge;
                    }
                }
                else if (Canvas.CurrentTool == SPTool.DrawEdge)
                {
                    // We're finishing an edge
                    try
                    {
                        Canvas.FinishEdge(this);
                        Canvas.RestorePreviousTool();
                    }
                    catch (ArgumentException)
                    {
                        //TODO something
                    }
                }
            }
        }

        internal void Register(SPLogicComponent parentComponent, SPConnectorType connectorType, int connectorId)
        {
            // Sanity checks
            Assert.IsNull(ParentComponent);
            Assert.AreEqual(connectorType, ConnectorType);

            ParentComponent = parentComponent;
            ConnectorId = connectorId;
        }

        private void Awake()
        {
        }

        private void Start()
        {
            Canvas = FindObjectOfType<SPCanvas>();
            Assert.IsNotNull(Canvas);
        }
    }
}
