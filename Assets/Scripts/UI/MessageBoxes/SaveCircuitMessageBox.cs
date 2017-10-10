using Assets.Scripts.Savefile;
using Assets.Scripts.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace Assets.Scripts.UI.MessageBoxes
{
    public class SaveCircuitMessageBox : UIMessageBox
    {
        public const string SAVE_OVERWRITE_MESSAGE_BOX_CONFIG_RESOURCE = "Configs/MessageBoxes/save_overwrite";
        public const string SAVE_BAD_NAME_MESSAGE_BOX_CONFIG_RESOURCE = "Configs/MessageBoxes/save_bad_name";
        private MessageBoxConfig SaveOverwriteMessageBoxConfig;
        private MessageBoxConfig SaveBadNameMessageBoxConfig;
        private UIMessageBoxFactory MessageBoxFactory;

        public override void Trigger(MessageBoxTriggerData triggerData)
        {
            if (triggerData.Sender.GetType() == typeof(SimpleMessageBox))
            {
                // Trigger from bad name message box
                Destroy(triggerData.Sender.gameObject);
            }
            else if (triggerData.Sender.GetType() == typeof(ConfirmMessageBox))
            {
                // Trigger from overwrite message box
                if (triggerData.ButtonPressed == MessageBoxButtonType.Positive)
                {
                    // Confirm overwrite
                    MessageBoxTriggerData newTriggerData = new MessageBoxTriggerData
                    {
                        ButtonPressed = MessageBoxButtonType.Positive,
                        Sender = this,
                        TextInput = this.TextInput.FindChildGameObject("UIMessageBoxTextInputText").GetComponent<Text>().text
                    };
                    TriggerTarget.Trigger(newTriggerData);
                }
                Destroy(triggerData.Sender.gameObject);
            }
        }

        public void OnSaveButtonClick()
        {
            string filename = this.TextInput.FindChildGameObject("UIMessageBoxTextInputText").GetComponent<Text>().text;
            string fullname = filename + ".json";
            string fullpath = Directories.SAVEFILE_FOLDER_FULL_PATH + "/" + fullname;

            // If file has invalid name, show error dialog
            if (filename.IndexOfAny(Path.GetInvalidFileNameChars()) != -1 ||
                String.IsNullOrWhiteSpace(filename))
            {
                MessageBoxFactory.MakeFromConfig(SaveBadNameMessageBoxConfig, this);
                return;
            }

            // If file already exists, ask for overwrite confirmation
            if (File.Exists(fullpath))
            {
                MessageBoxFactory.MakeFromConfig(SaveOverwriteMessageBoxConfig, this);
                return;
            }

            // Run callback to save circuit, callback will clean up message box
            MessageBoxTriggerData triggerData = new MessageBoxTriggerData
            {
                ButtonPressed = MessageBoxButtonType.Positive,
                Sender = this,
                TextInput = filename
            };
            TriggerTarget.Trigger(triggerData);
        }

        public void OnCancelButtonClick()
        {
            // Do nothing, close message box
            Destroy(gameObject);
        }

        private void Awake()
        {
            MessageBoxFactory = new UIMessageBoxFactory();

            // Load the message box config for save overwrite
            TextAsset configAsset = Resources.Load<TextAsset>(SAVE_OVERWRITE_MESSAGE_BOX_CONFIG_RESOURCE);
            Assert.IsNotNull(configAsset);
            SaveOverwriteMessageBoxConfig = JsonUtility.FromJson<MessageBoxConfig>(configAsset.text);

            // Load the message box config for bad name
            configAsset = Resources.Load<TextAsset>(SAVE_BAD_NAME_MESSAGE_BOX_CONFIG_RESOURCE);
            Assert.IsNotNull(configAsset);
            SaveBadNameMessageBoxConfig = JsonUtility.FromJson<MessageBoxConfig>(configAsset.text);
        }
    }
}
