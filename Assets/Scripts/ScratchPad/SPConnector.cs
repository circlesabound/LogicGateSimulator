using System;
using UnityEngine;
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
        protected abstract SPConnectorType ConnectorType
        {
            get;
        }

        public virtual void OnPointerClick(PointerEventData eventData)
        {
            // if somehow there ends up being more than two connector types
            // it might be better to refactor this functionality into the sub classes
            switch (ConnectorType)
            {
                case SPConnectorType.SPInConnector:
                    Debug.Log("Clicked on in connector");
                    break;

                case SPConnectorType.SPOutConnector:
                    Debug.Log("Clicked on out connector");
                    break;

                default:
                    throw new InvalidOperationException();
            }
        }
    }
}
