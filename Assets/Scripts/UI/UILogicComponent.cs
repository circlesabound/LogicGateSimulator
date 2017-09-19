using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.UI
{
    /// <summary>
    /// An abstract class that all UI representations of a logic component must extend.
    /// </summary>
    public abstract class UILogicComponent : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        //TODO: add a reference to the backend representation as an instance member
        protected object LogicComponent;

        protected UILogicComponent()
        {
            // Don't do anything here!
            // Initialisation happens in Start()
        }

        // Use this for initialisation
        void Start()
        {
            //TODO: attach a backend representation to this object
        }

        // Update is called once per frame
        void Update()
        {

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
