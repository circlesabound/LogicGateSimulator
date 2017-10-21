using Assets.Scripts.Savefile;
using Assets.Scripts.Util;
using System;
using System.IO;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace Assets.Scripts.UI.MessageBoxes
{
    public class SaveCircuitMessageBox : UIMessageBox
    {
        private const string SAVE_BAD_NAME_MESSAGE_BOX_CONFIG_RESOURCE = "Configs/MessageBoxes/bad_name";
        private const string SAVE_OVERWRITE_MESSAGE_BOX_CONFIG_RESOURCE = "Configs/MessageBoxes/save_overwrite";
        private UIMessageBoxFactory MessageBoxFactory;
        private MessageBoxConfig SaveBadNameMessageBoxConfig;
        private MessageBoxConfig SaveOverwriteMessageBoxConfig;

        public void OnCancelButtonClick()
        {
            // Run callback to close message box and unfreeze canvas
            MessageBoxTriggerData triggerData = new MessageBoxTriggerData
            {
                ButtonPressed = MessageBoxButtonType.Negative,
                Sender = this
            };
            TriggerTarget.Trigger(triggerData);
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

            // Run callback to save circuit, close message box, and unfreeze canvas
            MessageBoxTriggerData triggerData = new MessageBoxTriggerData
            {
                ButtonPressed = MessageBoxButtonType.Positive,
                Sender = this,
                TextInput = filename
            };
            TriggerTarget.Trigger(triggerData);
        }

        public void OnSaveAsChallengeButtonClick()
        {
            string filename = this.TextInput.FindChildGameObject("UIMessageBoxTextInputText").GetComponent<Text>().text;
            string fullname = filename + ".json";
            string fullpath = Directories.CHALLENGE_FOLDER_FULL_PATH + "/" + fullname;

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

            // Run callback to save circuit, close message box, and unfreeze canvas
            MessageBoxTriggerData triggerData = new MessageBoxTriggerData
            {
                ButtonPressed = MessageBoxButtonType.Neutral,
                Sender = this,
                TextInput = filename
            };
            TriggerTarget.Trigger(triggerData);
        }

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
