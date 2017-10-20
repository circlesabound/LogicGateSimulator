using Assets.Scripts.Savefile;
using Assets.Scripts.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;

namespace Assets.Scripts.ScratchPad
{
    /// <summary>
    /// An abstract class that all scratchpad representations of a logic component must extend.
    /// </summary>
    public abstract class SPLogicComponent : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
    {
        public LogicComponent LogicComponent;
        public SPInConnector SPInConnectorPrefab;
        public SPOutConnector SPOutConnectorPrefab;

        protected SPCanvas Canvas;
        public List<SPConnector> InConnectors;
        public List<SPConnector> OutConnectors;

        public Sprite UnselectedSprite;
        public Sprite SelectedSprite;

        protected SPLogicComponent()
        {
            // Don't do anything here!
            // Initialisation happens in Start()
        }

        public void Delete()
        {
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
            Canvas.Components.Remove(this);
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
                //    Enumerable
                //        .Concat(InConnectors, OutConnectors)
                //        .Select(c => c.ConnectedEdge)
                //        .Where(e => e != null)
                //        .ForEach(e => e.UpdatePosition());
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

            Assert.IsNotNull(SelectedSprite);
            Assert.IsNotNull(UnselectedSprite);

            InConnectors = new List<SPConnector>();
            OutConnectors = new List<SPConnector>();

            // We can probably assume canvas is ready by this point
            Canvas = FindObjectOfType<SPCanvas>();
            Assert.IsNotNull(Canvas);
        }

        // Use this for initialisation
        protected virtual void Start()
        {
        }

        // Update is called once per frame
        protected virtual void Update()
        {
        }

        /// <summary>
        /// Generate a config for this logic component.
        /// </summary>
        /// <returns>A config which can rebuild this logic component.</returns>
        public LogicComponentConfig GenerateConfig()
        {
            return GenerateConfig(Guid.NewGuid());
        }

        /// <summary>
        /// Generate a config for this logic component using a given GUID.
        /// </summary>
        /// <param name="guid">The GUID that the config will use for self-identification.</param>
        /// <returns>A config which can rebuild this logic component.</returns>
        public LogicComponentConfig GenerateConfig(Guid guid)
        {
            return new LogicComponentConfig(
                guid,
                this.GetType(),
                gameObject.transform.position.x,
                gameObject.transform.position.y);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode() ^ gameObject.transform.position.GetHashCode();
        }

        /// <summary>
        /// Linked in Unity inspector
        /// </summary>
        public virtual void OnPointerEnter(PointerEventData data)
        {
            Debug.Log("pointer enter");
            gameObject.GetComponent<SpriteRenderer>().sprite = SelectedSprite;
        }

        /// <summary>
        /// Linked in Unity inspector
        /// </summary>
        public virtual void OnPointerExit(PointerEventData data)
        {
            Debug.Log("pointer exit");
            gameObject.GetComponent<SpriteRenderer>().sprite = UnselectedSprite;
        }
    }
}
