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
        private SPCanvas Canvas;

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
                    // We're starting a new edge
                    Canvas.StartEdge(this);
                    Canvas.CurrentTool = SPTool.DrawEdge;
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

        private void Start()
        {
            Canvas = FindObjectOfType<SPCanvas>();
            Assert.IsNotNull(Canvas);
        }
    }
}
