using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.ScratchPad
{
    /// <summary>
    /// An abstract class that all UI representations of a logic component must extend.
    /// </summary>
    public abstract class SPLogicComponent : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        //TODO: add a reference to the backend representation as an instance member
        protected object LogicComponent;

        protected SPLogicComponent()
        {
            // Don't do anything here!
            // Initialisation happens in Start()
        }

        // Use this for initialisation
        private void Start()
        {
            //TODO: attach a backend representation to this object
        }

        // Update is called once per frame
        private void Update()
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
