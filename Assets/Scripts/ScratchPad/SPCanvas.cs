using Assets.Scripts.UI;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;

namespace Assets.Scripts.ScratchPad
{
    public class SPCanvas : MonoBehaviour, IPointerClickHandler
    {
        public GameObject foreground;
        public List<GameObject> Components;

        public void OnPointerClick(PointerEventData eventData)
        {
            UIToolboxComponentEntry chosenComponentEntry = GameObject.FindObjectOfType<UIToolboxContainer>().CurrentPanel.CurrentlySelectedEntry as UIToolboxComponentEntry;
            if (chosenComponentEntry == null)
            {
                return;
            }

            if (eventData.button == PointerEventData.InputButton.Left)
            {
                Debug.Log("Canvas| Left click");
                GameObject prefab = Resources.Load(chosenComponentEntry.Prefab) as GameObject;
                Assert.IsNotNull(prefab);
                GameObject newElem = GameObject.Instantiate(
                    prefab,
                    new Vector3(
                        eventData.pointerCurrentRaycast.worldPosition.x,
                        eventData.pointerCurrentRaycast.worldPosition.y),
                    Quaternion.identity,
                    foreground.transform);
                Assert.IsNotNull(newElem);
                newElem.tag = "SPElement";

                // will this memory leak?
                Components.Add(newElem);
            }
            else if (eventData.button == PointerEventData.InputButton.Right)
            {
                Debug.Log("Canvas| Right click");
            }
        }

        // Use this for initialization
        private void Awake()
        {
            Components = new List<GameObject>();
        }

        private void Start()
        {
            foreground = GameObject.Find("Foreground");
            Assert.IsNotNull(foreground);
        }

        // Update is called once per frame
        private void Update()
        {
        }
    }
}
