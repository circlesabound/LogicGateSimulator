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
    public class UIOverlayMenuOpenButton : MonoBehaviour, IMessageBoxTriggerTarget
    {
        private const string OPEN_CIRCUIT_MESSAGE_BOX_CONFIG_RESOURCE = "Configs/MessageBoxes/open";
        private const string OPEN_ERROR_MESSAGE_BOX_CONFIG_RESOURCE = "Configs/MessageBoxes/open_error";

        private SPCanvas Canvas;
        private UIMessageBoxFactory MessageBoxFactory;
        private MessageBoxConfig OpenCircuitMessageBoxConfig;
        private MessageBoxConfig OpenErrorMessageBoxConfig;

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
                // TODO here would be a good place to prompt unsaved changes

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

                // Build edges using GUID map
                foreach (var config in edgeConfigs)
                {
                    Canvas.StartEdge(guidMap[config.ComponentGuids[0]].InConnectors[config.connector_ids[0]]);
                    Canvas.FinishEdge(guidMap[config.ComponentGuids[1]].OutConnectors[config.connector_ids[1]]);
                }

                // Edges don't show without forcing an update
                foreach (var edge in Canvas.Edges)
                {
                    edge.UpdatePosition();
                }
            }

            Canvas.Frozen = false;
            Destroy(triggerData.Sender.gameObject);
        }

        private void Awake()
        {
            MessageBoxFactory = new UIMessageBoxFactory();

            // Load the message box config for open circuit
            TextAsset configAsset = Resources.Load<TextAsset>(OPEN_CIRCUIT_MESSAGE_BOX_CONFIG_RESOURCE);
            Assert.IsNotNull(configAsset);
            OpenCircuitMessageBoxConfig = JsonUtility.FromJson<MessageBoxConfig>(configAsset.text);

            // Load the message box config for open error
            configAsset = Resources.Load<TextAsset>(OPEN_ERROR_MESSAGE_BOX_CONFIG_RESOURCE);
            Assert.IsNotNull(configAsset);
            OpenErrorMessageBoxConfig = JsonUtility.FromJson<MessageBoxConfig>(configAsset.text);
        }

        private void Start()
        {
            Canvas = FindObjectOfType<SPCanvas>();
            Assert.IsNotNull(Canvas);
        }
    }
}
