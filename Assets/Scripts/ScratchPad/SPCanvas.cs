using Assets.Scripts.Savefile;
using Assets.Scripts.UI;
using Assets.Scripts.Util;
using System;
using System.Linq;
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
        public List<SPLogicComponent> Components;
        public List<SPEdge> Edges;
        public GameObject Foreground;
        public bool Frozen;
        public bool Running;
        public SPEdge SPEdgePrefab;
        private SPTool _CurrentTool;
        private SPTool _PreviousTool;

        public int ComponentsHash
        {
            get
            {
                return Components.Aggregate(0, (h, c) => h ^ c.GetHashCode());
            }
        }

        public int LastSavedComponentsHash;

        private SPLogicComponentFactory LogicComponentFactory;

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
            CurrentEdge.AddFinishingConnector(connector);
            CurrentEdge = null;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            Debug.Log("Canvas| " + this.CurrentTool.ToString() + " | " + eventData.button.ToString() + " click");
            if (CurrentTool == SPTool.Pointer)
            {
                // do nothing
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
                    // Build component configuration
                    LogicComponentConfig componentConfig = new LogicComponentConfig(
                        chosenComponentEntry.ComponentClassname,
                        eventData.pointerCurrentRaycast.worldPosition.x,
                        eventData.pointerCurrentRaycast.worldPosition.y);

                    // Pass config to factory
                    var newComponent = LogicComponentFactory.MakeFromConfig(componentConfig);

                    // will this memory leak?
                    Components.Add(newComponent);
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
            CurrentEdge.AddStartingConnector(connector);
        }

        // Use this for initialization
        private void Awake()
        {
            Assert.IsNotNull(Foreground);
            Components = new List<SPLogicComponent>();
            Edges = new List<SPEdge>();
            _CurrentTool = SPTool.Pointer;
            _PreviousTool = SPTool.Pointer;
            CurrentEdge = null;
            Circuit = new Circuit();
            Running = false;
            Frozen = false;
        }

        private void Start()
        {
            this.LogicComponentFactory = new SPLogicComponentFactory(this.Foreground);
        }

        // Update is called once per frame
        private void Update()
        {
            if (!Frozen)
            {
                if (Running) Circuit.Simulate();
                var scrollDelta = Input.GetAxis("Mouse ScrollWheel");
                CameraAdjust.SimpleZoom(scrollDelta);
            }
        }
    }
}
