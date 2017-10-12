using Assets.Scripts.Util;
using System.Collections.Generic;
using System.Linq;
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
        public LogicComponent LogicComponent;
        public SPInConnector SPInConnectorPrefab;
        public SPOutConnector SPOutConnectorPrefab;

        protected SPCanvas Canvas;
        protected List<SPConnector> InConnectors;
        protected List<SPConnector> OutConnectors;

        protected SPLogicComponent()
        {
            // Don't do anything here!
            // Initialisation happens in Start()
        }

        public void Delete()
        {
            Canvas.Components.Remove(this.gameObject);
            Canvas.Circuit.RemoveComponent(this.LogicComponent);
            Destroy(this.gameObject);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            //throw new NotImplementedException();
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (Canvas.CurrentTool == SPTool.Pointer)
            {
                this.gameObject.transform.position = Util.Util.MouseWorldCoordinates;
                Enumerable
                    .Concat(InConnectors, OutConnectors)
                    .Select(c => c.ConnectedEdge)
                    .Where(e => e != null)
                    .ForEach(e => e.UpdatePosition());
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            //throw new NotImplementedException();
        }

        public virtual void OnPointerClick(PointerEventData eventData)
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
                            .Select(c => c.ConnectedEdge)
                            .Where(e => e != null)
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
    }
}
