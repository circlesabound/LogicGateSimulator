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
    public class UIOverlayMenuChallengeButton : MonoBehaviour, IMessageBoxTriggerTarget, IInfoPanelTextProvider, IPointerEnterHandler, IPointerExitHandler
    {
        private const string CHALLENGE_BUTTON_INFO_PANEL_TITLE = "Load challenge";
        private const string CHALLENGE_INFO_PANEL_DESCRIPTION = "Attempt a circuit challenge.";
        private const string PLAY_CHALLENGE_MESSAGE_BOX_CONFIG_RESOURCE = "Configs/MessageBoxes/play_challenge";
        private const string OPEN_ERROR_MESSAGE_BOX_CONFIG_RESOURCE = "Configs/MessageBoxes/open_error";
        private const string UNSAVED_CHANGES_MESSAGE_BOX_CONFIG_RESOURCE = "Configs/MessageBoxes/unsaved_changes";

        private SPCanvas Canvas;
        private UIOverlayInfoPanel InfoPanel;
        private UIMessageBoxFactory MessageBoxFactory;
        private MessageBoxConfig PlayChallengeMessageBoxConfig;
        private MessageBoxConfig OpenErrorMessageBoxConfig;
        private MessageBoxConfig UnsavedChangesMessageBoxConfig;
        private Stack<string> SelectedFilenameStash; // to save filename data across confirmation message boxes

        public string InfoPanelTitle
        {
            get
            {
                return CHALLENGE_BUTTON_INFO_PANEL_TITLE;
            }
        }

        public string InfoPanelText
        {
            get
            {
                return CHALLENGE_INFO_PANEL_DESCRIPTION;
            }
        }

        /// <summary>
        /// Linked to button click in Unity inspector
        /// </summary>
        public void OnButtonClick()
        {
            Canvas.Frozen = true;
            MessageBoxFactory.MakeFromConfig(PlayChallengeMessageBoxConfig, this);
        }

        /// <summary>
        /// Callback to be executed after PlayChallengeMessageBox.
        /// Unfreezes the canvas and closes the message box.
        /// Positive trigger clears the current circuit and loads a challenge from JSON.
        /// </summary>
        /// <param name="triggerData">Data to pass from the trigger source to the trigger target.</param>
        public void Trigger(MessageBoxTriggerData triggerData)
        {
            if (triggerData.ButtonPressed == UIMessageBox.MessageBoxButtonType.Positive)
            {
                // Check for unsaved changes
                if (triggerData.Sender.GetType() == typeof(PlayChallengeMessageBox) &&
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

                string fullpath = Directories.CHALLENGE_FOLDER_FULL_PATH + "/" + triggerData.TextInput + ".json";

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
                var testCaseConfigs = circuitConfig.test_cases;
                var clockConfigs = circuitConfig.clocks;
                var titleText = circuitConfig.title_text;
                var bodyText = circuitConfig.body_text;
                var testCaseStepsRun = circuitConfig.test_case_steps_run;

                Assert.AreNotEqual(circuitConfig.game_mode, GameMode.Sandbox);

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

                // Set all initial components to immutable
                foreach (var component in Canvas.Components)
                {
                    component.Immutable = true;
                }

                foreach (var edge in Canvas.Edges)
                {
                    edge.Immutable = true;
                }

                // TODO: USE THE TEST CASES GENERATED.
                foreach (TestCaseConfig testCase in testCaseConfigs)
                {
                    foreach (KeyValuePair<uint, bool> kvp in testCase.GetAllInputs())
                    {
                        Debug.Log("Input: " + kvp.Key.ToString() + ": " + kvp.Value.ToString());
                    }
                    foreach (KeyValuePair<uint, bool> kvp in testCase.GetAllOutputs())
                    {
                        Debug.Log("Output: " + kvp.Key.ToString() + ": " + kvp.Value.ToString());
                    }
                }

                // Load the test cases into SPCanvas
                Canvas.TestCases = testCaseConfigs;

                // Load how many test case steps to run:
                Canvas.TestCaseStepsRun = testCaseStepsRun;

                // Put the canvas into challenge mode
                Canvas.CurrentMode = circuitConfig.game_mode;
                Canvas.ChallengeCompleted = false;
                Canvas.SetAsSaved();
                FindObjectOfType<UIOverlayControlVerifyChallengeButton>().GetComponent<RectTransform>().sizeDelta = new Vector2
                {
                    x = 40,
                    y = 40
                };

                // Set the Info Panel information.
                Canvas.InfoPanelTitle = titleText;
                Canvas.InfoPanelText = bodyText;
                InfoPanel.SetInfoTarget(Canvas);
                InfoPanel.Show();
            }

            Canvas.Frozen = false;
            Destroy(triggerData.Sender.gameObject);
        }

        private void Awake()
        {
            MessageBoxFactory = new UIMessageBoxFactory();
            SelectedFilenameStash = new Stack<string>();

            // Load the message box config for open circuit
            TextAsset configAsset = Resources.Load<TextAsset>(PLAY_CHALLENGE_MESSAGE_BOX_CONFIG_RESOURCE);
            Assert.IsNotNull(configAsset);
            PlayChallengeMessageBoxConfig = JsonUtility.FromJson<MessageBoxConfig>(configAsset.text);

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
