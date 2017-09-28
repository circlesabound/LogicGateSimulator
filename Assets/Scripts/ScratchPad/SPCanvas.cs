using Assets.Scripts.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.ScratchPad
{
    public class SPCanvas : MonoBehaviour, IPointerClickHandler
    {
        public GameObject foreground;
        public Stack<GameObject> children;

        public void OnPointerClick(PointerEventData eventData)
        {
            UIToolboxComponentEntry chosenTool = GameObject.FindObjectOfType<UIToolboxContainer>().CurrentPanel.CurrentlySelectedEntry as UIToolboxComponentEntry;
            if (chosenTool == null)
            {
                return;
            }

            if (eventData.button == PointerEventData.InputButton.Left)
            {
                Debug.Log("Canvas| Left click");
                GameObject prefab = Resources.Load(chosenTool.Prefab) as GameObject;
                GameObject newElem = GameObject.Instantiate(prefab, eventData.pointerCurrentRaycast.worldPosition, Quaternion.identity);
                newElem.tag = "SPElement";

                // do this for now
                newElem.transform.SetParent(foreground.transform);
                Vector3 newPos = newElem.transform.localPosition;
                newPos.z = 0;
                newElem.transform.localPosition = newPos;

                // will this memory leak?
                children.Push(newElem);
            }
            else if (eventData.button == PointerEventData.InputButton.Right)
            {
                Debug.Log("Canvas| Right click");

                try
                {
                    GameObject oldElem = children.Pop();
                    Destroy(oldElem);
                } catch (InvalidOperationException e)
                {
                    Debug.Log("No children: " + e);
                }
            }
        }

        // Use this for initialization
        void Awake()
        {
            children = new Stack<GameObject>();
        }

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
