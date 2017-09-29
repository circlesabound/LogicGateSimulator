using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;
using Assets.Scripts.ExtensionMethods;

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

        public void Delete()
        {
            Canvas.Components.Remove(this.gameObject);
            Canvas.Circuit.RemoveComponent(this.LogicComponent);
            Destroy(this.gameObject);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            Debug.Log(this.GetType().Name + "| " + Canvas.CurrentTool.ToString() + " | " + eventData.button.ToString() + " click");
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                //
            }
            else if (eventData.button == PointerEventData.InputButton.Right)
            {
                switch (Canvas.CurrentTool)
                {
                    case SPTool.Pointer:
                        // Delete any incoming/outgoing edges
                        var edgeList = Enumerable
                            .Concat(InConnectors, OutConnectors)
                            .SelectMany(c => c.ConnectedEdges)
                            .ToList();
                        for (int i = 0; i < edgeList.Count; ++i)
                        {
                            edgeList[i].Delete();
                        }

                        // Delete myself
                        Delete();
                        break;

                    default:
                        // do nothing
                        break;
                }
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
