using Assets.Scripts.Savefile;
using Assets.Scripts.ScratchPad;
using Assets.Scripts.UI.MessageBoxes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

namespace Assets.Scripts.UI
{
    public class UIOverlayMenuChallengeButton : MonoBehaviour, IMessageBoxTriggerTarget
    {
        private const string PLAY_CHALLENGE_MESSAGE_BOX_CONFIG_RESOURCE = "Configs/MessageBoxes/play_challenge";
        private const string OPEN_ERROR_MESSAGE_BOX_CONFIG_RESOURCE = "Configs/MessageBoxes/open_error";
        private const string UNSAVED_CHANGES_MESSAGE_BOX_CONFIG_RESOURCE = "Configs/MessageBoxes/unsaved_changes";

        private SPCanvas Canvas;
        private UIMessageBoxFactory MessageBoxFactory;
        private MessageBoxConfig PlayChallengeMessageBoxConfig;
        private MessageBoxConfig OpenErrorMessageBoxConfig;
        private MessageBoxConfig UnsavedChangesMessageBoxConfig;
        private Stack<string> SelectedFilenameStash; // to save filename data across confirmation message boxes

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
                var clockConfigs = circuitConfig.clocks;

                Assert.AreEqual(circuitConfig.game_mode, GameMode.ActivateAllOutputsChallenge);

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

                // Put the canvas into challenge mode
                Canvas.CurrentMode = GameMode.ActivateAllOutputsChallenge;
                Canvas.ChallengeCompleted = false;
                Canvas.SetAsSaved();
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
        }
    }
}
