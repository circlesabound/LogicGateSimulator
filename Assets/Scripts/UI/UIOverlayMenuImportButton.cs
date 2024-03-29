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
    public class UIOverlayMenuImportButton : MonoBehaviour, IMessageBoxTriggerTarget, IInfoPanelTextProvider, IPointerEnterHandler, IPointerExitHandler
    {
        private const string IMPORT_CIRCUIT_MESSAGE_BOX_CONFIG_RESOURCE = "Configs/MessageBoxes/import";
        private const string IMPORT_ERROR_MESSAGE_BOX_CONFIG_RESOURCE = "Configs/MessageBoxes/import_error";

        private const string IMPORT_BUTTON_INFO_PANEL_TITLE = "Import circuit";
        private const string IMPORT_BUTTON_INFO_PANEL_DESCRIPTION = "Import the components of a saved circuit into the current circuit.";

        private SPCanvas Canvas;
        private UIMessageBoxFactory MessageBoxFactory;
        private MessageBoxConfig ImportCircuitMessageBoxConfig;
        private MessageBoxConfig ImportErrorMessageBoxConfig;

        private UIOverlayInfoPanel InfoPanel;

        public string InfoPanelTitle
        {
            get
            {
                return IMPORT_BUTTON_INFO_PANEL_TITLE;
            }
        }

        public string InfoPanelText
        {
            get
            {
                return IMPORT_BUTTON_INFO_PANEL_DESCRIPTION;
            }
        }

        /// <summary>
        /// Linked to button click in Unity inspector
        /// </summary>
        public void OnButtonClick()
        {
            if (Canvas.IsChallenge) return;
            Canvas.Frozen = true;
            MessageBoxFactory.MakeFromConfig(ImportCircuitMessageBoxConfig, this);
        }

        /// <summary>
        /// Callback to be executed after OpenCircuitMessageBox.
        /// Unfreezes the canvas and closes the message box.
        /// Positive trigger loads a saved circuit from JSON.
        /// </summary>
        /// <param name="triggerData">Data to pass from the trigger source to the trigger target.</param>
        public void Trigger(MessageBoxTriggerData triggerData)
        {
            if (triggerData.ButtonPressed == UIMessageBox.MessageBoxButtonType.Positive)
            {
                string fullpath = Directories.SAVEFILE_FOLDER_FULL_PATH + "/" + triggerData.TextInput + ".json";

                // Read from file
                Debug.Log("Importing circuit from " + fullpath);
                string data;
                try
                {
                    data = File.ReadAllText(fullpath);
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                    MessageBoxFactory.MakeFromConfig(ImportErrorMessageBoxConfig);
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
            }

            Canvas.Frozen = false;
            Destroy(triggerData.Sender.gameObject);
        }

        private void Awake()
        {
            MessageBoxFactory = new UIMessageBoxFactory();

            // Load the message box config for import circuit
            TextAsset configAsset = Resources.Load<TextAsset>(IMPORT_CIRCUIT_MESSAGE_BOX_CONFIG_RESOURCE);
            Assert.IsNotNull(configAsset);
            ImportCircuitMessageBoxConfig = JsonUtility.FromJson<MessageBoxConfig>(configAsset.text);

            // Load the message box config for import error
            configAsset = Resources.Load<TextAsset>(IMPORT_ERROR_MESSAGE_BOX_CONFIG_RESOURCE);
            Assert.IsNotNull(configAsset);
            ImportErrorMessageBoxConfig = JsonUtility.FromJson<MessageBoxConfig>(configAsset.text);
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
