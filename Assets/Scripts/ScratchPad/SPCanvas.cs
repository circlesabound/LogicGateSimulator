using Assets.Scripts.Savefile;
using Assets.Scripts.UI;
using Assets.Scripts.Util;
using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;
using Assets.Scripts.UI.MessageBoxes;

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

    public class SPCanvas : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        private const string CHALLENGE_COMPLETE_MESSAGE_BOX_CONFIG_RESOURCE = "Configs/MessageBoxes/challenge_complete";

        private MessageBoxConfig ChallengeCompleteMessageBoxConfig;

        public List<SPLogicComponent> Components;
        public List<SPEdge> Edges;
        public GameObject Foreground;
        public bool Frozen;
        private int LastSavedComponentsHash;
        public bool Running;
        public SPEdge SPEdgePrefab;
        private SPTool _CurrentTool;
        private SPTool _PreviousTool;
        public bool IsChallenge;
        public bool ChallengeCompleted;

        public bool IsUnsaved
        {
            get
            {
                return !IsChallenge && ComponentsHash != LastSavedComponentsHash;
            }
        }

        public void SetAsSaved()
        {
            LastSavedComponentsHash = ComponentsHash;
        }

        private SPLogicComponentFactory LogicComponentFactory;
        private UIMessageBoxFactory MessageBoxFactory;

        public Circuit Circuit
        {
            get;
            private set;
        }

        private int ComponentsHash
        {
            get
            {
                return Components.Aggregate(0, (h, c) => h ^ c.GetHashCode());
            }
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

        public void OnBeginDrag(PointerEventData eventData)
        {
            //TODO maybe change cursor
            //throw new NotImplementedException();
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Right)
            {
                CameraAdjust.Pan(-eventData.delta / gameObject.transform.localScale.x);
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            //TODO maybe change cursor
            //throw new NotImplementedException();
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
                    // Can't build in challenges
                    if (IsChallenge) return;

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

            // Load the message box config for open circuit
            TextAsset configAsset = Resources.Load<TextAsset>(CHALLENGE_COMPLETE_MESSAGE_BOX_CONFIG_RESOURCE);
            Assert.IsNotNull(configAsset);
            ChallengeCompleteMessageBoxConfig = JsonUtility.FromJson<MessageBoxConfig>(configAsset.text);
        }

        private void Start()
        {
            LogicComponentFactory = new SPLogicComponentFactory(Foreground);
            MessageBoxFactory = new UIMessageBoxFactory();
        }

        // Update is called once per frame
        private void Update()
        {
            if (!Frozen)
            {
                if (Running) Circuit.Simulate();
                var scrollDelta = Input.GetAxis("Mouse ScrollWheel");
                CameraAdjust.SimpleZoom(scrollDelta);

                if (IsChallenge && !ChallengeCompleted)
                {
                    bool challengeComplete = false; //TODO check with backend
                    if (challengeComplete)
                    {
                        MessageBoxFactory.MakeFromConfig(ChallengeCompleteMessageBoxConfig);
                        ChallengeCompleted = true;
                    }
                }
            }
        }
    }
}
