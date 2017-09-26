using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.ScratchPad
{
    public class SPElement : MonoBehaviour, IPointerClickHandler
    {

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

        // Use this for initialization
        void Awake()
        {
        }

        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
        }
    }
}
