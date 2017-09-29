using Assets.Scripts.UI;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;

namespace Assets.Scripts.ScratchPad
{
    public enum SPTool
    {
        // "Normal" tools

        Pointer,
        Pan,
        Zoom,

        // "Special" tools

        NewComponent,
        DrawEdge
    }

    public class SPCanvas : MonoBehaviour, IPointerClickHandler
    {
        public List<GameObject> Components;
        public List<GameObject> Edges;
        public GameObject Foreground;
        public SPEdge SPEdgePrefab;
        private SPTool _CurrentTool;
        private SPTool _PreviousTool;

        public Circuit Circuit
        {
            get;
            private set;
        }

        public SPEdge CurrentEdge
        {
            get;
            private set;
        }

        public SPTool CurrentTool
        {
            get
            {
                return this._CurrentTool;
            }
            set
            {
                switch (value)
                {
                    case SPTool.Pan:
                    case SPTool.Zoom:
                        throw new NotImplementedException();
                    default:
                        if (this._CurrentTool == SPTool.Pointer || this._CurrentTool == SPTool.Pan || this._CurrentTool == SPTool.Zoom)
                        {
                            this._PreviousTool = this._CurrentTool;
                        }
                        this._CurrentTool = value;
                        break;
                }
            }
        }

        public void FinishEdge(SPConnector connector)
        {
            // this may throw ArgumentException, let SPConnector.OnPointerClick handle that
            CurrentEdge.AddConnector(connector);

            CurrentEdge.Finalise();
            this.Edges.Add(CurrentEdge.gameObject);
            CurrentEdge = null;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            Debug.Log("Canvas| " + this.CurrentTool.ToString() + " | " + eventData.button.ToString() + " click");
            if (CurrentTool == SPTool.Pointer)
            {
                Circuit.Simulate(); //TODO move this somewhere else
            }
            else if (CurrentTool == SPTool.Pan)
            {
                throw new NotImplementedException();
            }
            else if (CurrentTool == SPTool.Zoom)
            {
                throw new NotImplementedException();
            }
            else if (CurrentTool == SPTool.NewComponent)
            {
                UIToolboxComponentEntry chosenComponentEntry = GameObject.FindObjectOfType<UIToolboxContainer>().CurrentPanel.CurrentlySelectedEntry as UIToolboxComponentEntry;
                Assert.IsNotNull(chosenComponentEntry);

                if (eventData.button == PointerEventData.InputButton.Left)
                {
                    GameObject prefab = Resources.Load(chosenComponentEntry.Prefab) as GameObject;
                    Assert.IsNotNull(prefab);
                    GameObject newElem = GameObject.Instantiate(
                        prefab,
                        new Vector3(
                            eventData.pointerCurrentRaycast.worldPosition.x,
                            eventData.pointerCurrentRaycast.worldPosition.y),
                        Quaternion.identity,
                        Foreground.transform);
                    Assert.IsNotNull(newElem);
                    newElem.tag = "SPElement";

                    // will this memory leak?
                    Components.Add(newElem);
                }
                else if (eventData.button == PointerEventData.InputButton.Right)
                {
                    //
                }
            }
            else if (CurrentTool == SPTool.DrawEdge)
            {
                if (eventData.button == PointerEventData.InputButton.Right)
                {
                    // Cancel drawing edge
                    Assert.IsNotNull(CurrentEdge);
                    CurrentEdge.Delete();
                    RestorePreviousTool();
                }
            }
        }

        public void RestorePreviousTool()
        {
            this.CurrentTool = this._PreviousTool;
        }

        public void StartEdge(SPConnector connector)
        {
            CurrentEdge = Instantiate(SPEdgePrefab, Foreground.transform);
            Assert.IsNotNull(CurrentEdge);
            CurrentEdge.AddConnector(connector);
        }

        // Use this for initialization
        private void Awake()
        {
            Assert.IsNotNull(Foreground);
            Components = new List<GameObject>();
            Edges = new List<GameObject>();
            _CurrentTool = SPTool.Pointer;
            _PreviousTool = SPTool.Pointer;
            CurrentEdge = null;
            Circuit = new Circuit();
        }

        private void Start()
        {
            //Foreground = GameObject.Find("Foreground"); // Linked in Unity inspector
        }

        // Update is called once per frame
        private void Update()
        {
        }
    }
}
