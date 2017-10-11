using Assets.Scripts.Savefile;
using Assets.Scripts.ScratchPad;
using Assets.Scripts.UI.MessageBoxes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;

namespace Assets.Scripts.UI
{
    public class UIOverlayMenuOpenButton : MonoBehaviour, IMessageBoxTriggerTarget
    {
        private const string OPEN_CIRCUIT_MESSAGE_BOX_CONFIG_RESOURCE = "Configs/MessageBoxes/open";

        private SPCanvas Canvas;
        private UIMessageBoxFactory MessageBoxFactory;
        private MessageBoxConfig OpenCircuitMessageBoxConfig;

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
                string fullpath = Directories.SAVEFILE_FOLDER_FULL_PATH + "/" + triggerData.TextInput + ".json";
                Debug.Log("Opening circuit from " + fullpath);
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
        }

        private void Start()
        {
            Canvas = FindObjectOfType<SPCanvas>();
            Assert.IsNotNull(Canvas);
        }
    }
}
