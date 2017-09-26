using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.ScratchPad
{
    public class SPCanvas : MonoBehaviour, IPointerClickHandler
    {
        public GameObject elementPrefab;
        public GameObject foreground;

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                Debug.Log("Left click");
                GameObject newElem = GameObject.Instantiate(elementPrefab, eventData.pointerCurrentRaycast.worldPosition, Quaternion.identity);
                newElem.transform.SetParent(foreground.transform);
                newElem.transform.localScale = new Vector3(0.1f, 0.1f, 1);
            }
            else if (eventData.button == PointerEventData.InputButton.Right)
            {
                Debug.Log("Right click");
                // open the right click menu here --> some youtube link
                // this https://unity3d.com/learn/tutorials/modules/intermediate/live-training-archive/panels-panes-windows
            }
        }

        // Use this for initialization
        void Start()
        {
            foreground = GameObject.Find("Foreground");
        }

        // Update is called once per frame
        void Update()
        {
        }
    }
}
