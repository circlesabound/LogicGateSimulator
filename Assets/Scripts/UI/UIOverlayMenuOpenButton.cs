﻿using Assets.Scripts.Savefile;
using Assets.Scripts.ScratchPad;
using Assets.Scripts.UI.MessageBoxes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;

namespace Assets.Scripts.UI
{
    public class UIOverlayMenuOpenButton : MonoBehaviour, IMessageBoxTriggerTarget, IInfoPanelTextProvider, IPointerEnterHandler, IPointerExitHandler
    {
        private const string OPEN_BUTTON_INFO_PANEL_TITLE = "Open circuit";
        private const string OPEN_BUTTON_INFO_PANEL_DESCRIPTION = "Open a saved circuit file.";

        private const string OPEN_CIRCUIT_MESSAGE_BOX_CONFIG_RESOURCE = "Configs/MessageBoxes/open";
        private const string OPEN_ERROR_MESSAGE_BOX_CONFIG_RESOURCE = "Configs/MessageBoxes/open_error";
        private const string UNSAVED_CHANGES_MESSAGE_BOX_CONFIG_RESOURCE = "Configs/MessageBoxes/unsaved_changes";

        private UIOverlayInfoPanel InfoPanel;

        private SPCanvas Canvas;
        private UIMessageBoxFactory MessageBoxFactory;
        private MessageBoxConfig OpenCircuitMessageBoxConfig;
        private MessageBoxConfig OpenErrorMessageBoxConfig;
        private MessageBoxConfig UnsavedChangesMessageBoxConfig;
        private Stack<string> SelectedFilenameStash; // to save filename data across confirmation message boxes

        public string InfoPanelTitle
        {
            get
            {
                return OPEN_BUTTON_INFO_PANEL_TITLE;
            }
        }

        public string InfoPanelText
        {
            get
            {
                return OPEN_BUTTON_INFO_PANEL_DESCRIPTION;
            }
        }

        /// <summary>
        /// Linked to button click in Unity inspector
        /// </summary>
        public void OnButtonClick()
        {
            Canvas.Frozen = true;
            MessageBoxFactory.MakeFromConfig(OpenCircuitMessageBoxConfig, this);
        }

        /// <summary>
        /// Callback to be executed after OpenCircuitMessageBox.
        /// Unfreezes the canvas and closes the message box.
        /// Positive trigger clears the current circuit and loads a saved one from JSON.
        /// </summary>
        /// <param name="triggerData">Data to pass from the trigger source to the trigger target.</param>
        public void Trigger(MessageBoxTriggerData triggerData)
        {
            if (triggerData.ButtonPressed == UIMessageBox.MessageBoxButtonType.Positive)
            {
                // Check for unsaved changes
                if (triggerData.Sender.GetType() == typeof(OpenCircuitMessageBox) &&
                    Canvas.IsUnsaved)
                {
                    // Ask for confirmation of unsaved changes, save filename
                    SelectedFilenameStash.Push(triggerData.TextInput);
                    MessageBoxFactory.MakeFromConfig(UnsavedChangesMessageBoxConfig, this);
                    Destroy(triggerData.Sender.gameObject);
                    return;
                }
                else if (triggerData.Sender.GetType() == typeof(ConfirmMessageBox))
                {
                    // Confirmation received, pop filename
                    triggerData.TextInput = SelectedFilenameStash.Pop();
                    SelectedFilenameStash.Clear();
                }

                string fullpath = Directories.SAVEFILE_FOLDER_FULL_PATH + "/" + triggerData.TextInput + ".json";

                // Read from file
                Debug.Log("Opening circuit from " + fullpath);
                string data;
                try
                {
                    data = File.ReadAllText(fullpath);
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                    MessageBoxFactory.MakeFromConfig(OpenErrorMessageBoxConfig);
                    Canvas.Frozen = false;
                    Destroy(triggerData.Sender.gameObject);
                    return;
                }

                // Parse JSON into configs
                var circuitConfig = JsonUtility.FromJson<CircuitConfig>(data);
                var componentConfigs = circuitConfig.logic_components;
                var edgeConfigs = circuitConfig.edges;
                var togglerConfigs = circuitConfig.toggles;
                var numberedTogglerConfigs = circuitConfig.numbered_toggles;
                var clockConfigs = circuitConfig.clocks;

                Assert.AreEqual(circuitConfig.game_mode, GameMode.Sandbox);

                // Clear the canvas
                for (int i = Canvas.Components.Count - 1; i >= 0; --i)
                {
                    Canvas.Components[i].Delete();
                }

                var logicComponentFactory = new SPLogicComponentFactory(Canvas.Foreground);

                // Build components whilst populating GUID map
                Dictionary<Guid, SPLogicComponent> guidMap = componentConfigs.ToDictionary(
                    config => config.Guid,
                    config => logicComponentFactory.MakeFromConfig(config));
                Canvas.Components.AddRange(guidMap.Values);

                // Restore state for input togglers
                foreach (var config in togglerConfigs)
                {
                    Guid guid = Guid.Parse(config.guid_string);
                    SPInputToggler inputToggler = (SPInputToggler)guidMap[guid];
                    if (((InputComponent)inputToggler.LogicComponent).value != config.value)
                    {
                        inputToggler.ToggleValue();
                    }
                }

                foreach (var config in numberedTogglerConfigs)
                {
                    Guid guid = Guid.Parse(config.guid_string);
                    SPNumberedInputToggler inputToggler = (SPNumberedInputToggler)guidMap[guid];
                    if (((InputComponent)inputToggler.LogicComponent).value != config.value)
                    {
                        inputToggler.ToggleValue();
                    }
                }

                // Restore state for clock components
                foreach (var config in clockConfigs)
                {
                    Guid guid = Guid.Parse(config.guid_string);
                    SPClock clock = (SPClock)guidMap[guid];
                    ((Clock)clock.LogicComponent).Period = config.period;
                }

                // Build edges using GUID map
                foreach (var config in edgeConfigs)
                {
                    Canvas.StartEdge(guidMap[config.ComponentGuids[0]].InConnectors[config.connector_ids[0]]);
                    Canvas.FinishEdge(guidMap[config.ComponentGuids[1]].OutConnectors[config.connector_ids[1]]);
                }

                // Loading a circuit means the circuit that was just loaded is "saved"
                Canvas.CurrentMode = GameMode.Sandbox;
                Canvas.SetAsSaved();
                FindObjectOfType<UIOverlayControlVerifyChallengeButton>().GetComponent<RectTransform>().sizeDelta = new Vector2
                {
                    x = 0,
                    y = 0
                };
            }

            Canvas.Frozen = false;
            Destroy(triggerData.Sender.gameObject);
        }

        private void Awake()
        {
            MessageBoxFactory = new UIMessageBoxFactory();
            SelectedFilenameStash = new Stack<string>();

            // Load the message box config for open circuit
            TextAsset configAsset = Resources.Load<TextAsset>(OPEN_CIRCUIT_MESSAGE_BOX_CONFIG_RESOURCE);
            Assert.IsNotNull(configAsset);
            OpenCircuitMessageBoxConfig = JsonUtility.FromJson<MessageBoxConfig>(configAsset.text);

            // Load the message box config for open error
            configAsset = Resources.Load<TextAsset>(OPEN_ERROR_MESSAGE_BOX_CONFIG_RESOURCE);
            Assert.IsNotNull(configAsset);
            OpenErrorMessageBoxConfig = JsonUtility.FromJson<MessageBoxConfig>(configAsset.text);

            // Load the message box config for unsaved changes
            configAsset = Resources.Load<TextAsset>(UNSAVED_CHANGES_MESSAGE_BOX_CONFIG_RESOURCE);
            Assert.IsNotNull(configAsset);
            UnsavedChangesMessageBoxConfig = JsonUtility.FromJson<MessageBoxConfig>(configAsset.text);
        }

        private void Start()
        {
            Canvas = FindObjectOfType<SPCanvas>();
            Assert.IsNotNull(Canvas);
            InfoPanel = FindObjectOfType<UIOverlayInfoPanel>();
            Assert.IsNotNull(InfoPanel);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            InfoPanel.SetInfoTarget(this);
            InfoPanel.Show();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            InfoPanel.Hide();
        }
    }
}
