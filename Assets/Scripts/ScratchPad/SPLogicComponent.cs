using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;

namespace Assets.Scripts.ScratchPad
{
    /// <summary>
    /// An abstract class that all UI representations of a logic component must extend.
    /// </summary>
    public abstract class SPLogicComponent : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public SPInConnector SPInConnectorPrefab;
        public SPOutConnector SPOutConnectorPrefab;

        protected List<SPConnector> InConnectors;
        protected List<SPConnector> OutConnectors;

        public LogicComponent LogicComponent;

        protected SPCanvas Canvas;

        protected SPLogicComponent()
        {
            // Don't do anything here!
            // Initialisation happens in Start()
        }

        protected virtual void Awake()
        {
            // Sanity checks, make sure prefabs are linked
            Assert.raiseExceptions = true;
            Assert.IsNotNull(SPInConnectorPrefab);
            Assert.IsNotNull(SPOutConnectorPrefab);
            Assert.raiseExceptions = false;

            InConnectors = new List<SPConnector>();
            OutConnectors = new List<SPConnector>();
        }

        // Use this for initialisation
        protected virtual void Start()
        {
            Canvas = FindObjectOfType<SPCanvas>();
            Assert.IsNotNull(Canvas);
        }

        // Update is called once per frame
        protected virtual void Update()
        {
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                Debug.Log("Element| Left click");

                // start drawing a line for connection?
            }
            else if (eventData.button == PointerEventData.InputButton.Right)
            {
                Debug.Log("Element| Right click");

                // open the right click menu here --> some youtube link
                // this https://unity3d.com/learn/tutorials/modules/intermediate/live-training-archive/panels-panes-windows
            }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            throw new NotImplementedException();
        }

        public void OnDrag(PointerEventData eventData)
        {
            throw new NotImplementedException();
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            throw new NotImplementedException();
        }
    }
}
