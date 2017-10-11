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
    public class UIOverlayMenuSaveButton : MonoBehaviour, IMessageBoxTriggerTarget
    {
        private const string SAVE_CIRCUIT_MESSAGE_BOX_CONFIG_RESOURCE = "Configs/MessageBoxes/save";
        private const string SAVE_ERROR_MESSAGE_BOX_CONFIG_RESOURCE = "Configs/MessageBoxes/save_error";
        private SPCanvas Canvas;
        private UIMessageBoxFactory MessageBoxFactory;
        private MessageBoxConfig SaveCircuitMessageBoxConfig;
        private MessageBoxConfig SaveErrorMessageBoxConfig;

        /// <summary>
        /// Linked to button click in Unity inspector
        /// </summary>
        public void OnButtonClick()
        {
            Canvas.Frozen = true;
            MessageBoxFactory.MakeFromConfig(SaveCircuitMessageBoxConfig, this);
        }

        /// <summary>
        /// Callback to be executed after SaveCircuitMessageBox.
        /// Unfreezes the canvas and closes the message box.
        /// Positive trigger saves the circuit as JSON.
        /// </summary>
        /// <param name="triggerData">Data to pass from the trigger source to the trigger target.</param>
        public void Trigger(MessageBoxTriggerData triggerData)
        {
            if (triggerData.ButtonPressed == UIMessageBox.MessageBoxButtonType.Positive)
            {
                // Bind a GUID for each component
                Dictionary<SPLogicComponent, Guid> guidMap = Canvas.Components
                    .ToDictionary(c => c, c => Guid.NewGuid());

                // Generate configs for components
                List<LogicComponentConfig> componentConfigs = guidMap
                    .Select(kvp => kvp.Key.GenerateConfig(kvp.Value))
                    .ToList();

                // Generate configs for edges, mapping connected components using GUIDs
                List<EdgeConfig> edgeConfigs = Canvas.Edges
                    .Select(e => e.GenerateConfig(guidMap))
                    .ToList();

                // Build savefile
                CircuitConfig spConfig = new CircuitConfig(componentConfigs, edgeConfigs);
#if DEVELOPMENT_BUILD
                string saveData = JsonUtility.ToJson(spConfig, prettyPrint: true);
#else
            string saveData = JsonUtility.ToJson(spConfig, prettyPrint: false);
#endif

                // Build full file path
                string filename = triggerData.TextInput + ".json";
                string fullpath = Directories.SAVEFILE_FOLDER_FULL_PATH + "/" + filename;

                // Write to file
                Debug.Log("Saving to " + fullpath);
                try
                {
                    Directory.CreateDirectory(Directories.SAVEFILE_FOLDER_FULL_PATH);
                    File.WriteAllText(fullpath, saveData);
                }
                catch (Exception e)
                {
                    // uh oh
                    Debug.LogException(e);
                    MessageBoxFactory.MakeFromConfig(SaveErrorMessageBoxConfig);
                }
            }

            Canvas.Frozen = false;
            Destroy(triggerData.Sender.gameObject);
        }

        private void Awake()
        {
            MessageBoxFactory = new UIMessageBoxFactory();

            // Load the message box config for save ciruit
            TextAsset configAsset = Resources.Load<TextAsset>(SAVE_CIRCUIT_MESSAGE_BOX_CONFIG_RESOURCE);
            Assert.IsNotNull(configAsset);
            SaveCircuitMessageBoxConfig = JsonUtility.FromJson<MessageBoxConfig>(configAsset.text);

            // Load the message box config for save error
            configAsset = Resources.Load<TextAsset>(SAVE_ERROR_MESSAGE_BOX_CONFIG_RESOURCE);
            Assert.IsNotNull(configAsset);
            SaveErrorMessageBoxConfig = JsonUtility.FromJson<MessageBoxConfig>(configAsset.text);
        }

        private void Start()
        {
            Canvas = FindObjectOfType<SPCanvas>();
            Assert.IsNotNull(Canvas);
        }
    }
}
