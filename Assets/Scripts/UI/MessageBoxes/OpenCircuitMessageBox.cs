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
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts.UI.MessageBoxes
{
    public class OpenCircuitMessageBox : UIMessageBox, IScrollViewItemProvider
    {
        private const string OPEN_BAD_NAME_MESSAGE_BOX_CONFIG_RESOURCE = "Configs/MessageBoxes/bad_name";
        private const string OPEN_NO_FILE_MESSAGE_BOX_CONFIG_RESOURCE = "Configs/MessageBoxes/open_no_file";
        private UIMessageBoxFactory MessageBoxFactory;
        private MessageBoxConfig OpenBadNameMessageBoxConfig;
        private MessageBoxConfig OpenNoFileMessageBoxConfig;

        private IEnumerable<OpenCircuitScrollViewItem> Entries
        {
            get
            {
                return gameObject.GetComponentsInChildren<OpenCircuitScrollViewItem>();
            }
        }

        private OpenCircuitScrollViewItem CurrentlySelectedEntry;

        public override void Trigger(MessageBoxTriggerData triggerData)
        {
            // Only accept triggers from errors i.e. SimpleMessageBox
            if (triggerData.Sender.GetType() == typeof(SimpleMessageBox))
            {
                Destroy(triggerData.Sender.gameObject);
            }
        }

        public void OnOpenButtonClick()
        {
            // If no selection, do nothing
            if (CurrentlySelectedEntry == null)
            {
                return;
            }

            string filename = CurrentlySelectedEntry.Text.GetComponent<Text>().text;
            string fullname = filename + ".json";
            string fullpath = Directories.SAVEFILE_FOLDER_FULL_PATH + "/" + fullname;

            // If file has invalid name, show error dialog
            if (filename.IndexOfAny(Path.GetInvalidFileNameChars()) != -1 ||
                String.IsNullOrWhiteSpace(filename))
            {
                MessageBoxFactory.MakeFromConfig(OpenBadNameMessageBoxConfig, this);
                return;
            }

            // If file does not exist, show error dialog
            if (!File.Exists(fullpath))
            {
                MessageBoxFactory.MakeFromConfig(OpenNoFileMessageBoxConfig, this);
                return;
            }

            // Run callback to open circuit, close message box, and unfreeze canvas
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
            // Run callback to close message box and unfreeze canvas
            MessageBoxTriggerData triggerData = new MessageBoxTriggerData
            {
                ButtonPressed = MessageBoxButtonType.Negative,
                Sender = this
            };
            TriggerTarget.Trigger(triggerData);
        }

        public IEnumerable<string> EnumerateScrollViewItems()
        {
            if (Directory.Exists(Directories.SAVEFILE_FOLDER_FULL_PATH))
            {
                return Directory.EnumerateFiles(Directories.SAVEFILE_FOLDER_FULL_PATH)
                    .Where(fullpath => fullpath.EndsWith(".json"))
                    .Select(fullpath => Path.GetFileNameWithoutExtension(fullpath))
                    .OrderBy(name => name);
            }
            else
            {
                return Enumerable.Empty<string>();
            }
        }

        private void Awake()
        {
            MessageBoxFactory = new UIMessageBoxFactory();

            // Load the message box config for bad name
            TextAsset configAsset = Resources.Load<TextAsset>(OPEN_BAD_NAME_MESSAGE_BOX_CONFIG_RESOURCE);
            Assert.IsNotNull(configAsset);
            OpenBadNameMessageBoxConfig = JsonUtility.FromJson<MessageBoxConfig>(configAsset.text);

            // Load the message box config for no file
            configAsset = Resources.Load<TextAsset>(OPEN_NO_FILE_MESSAGE_BOX_CONFIG_RESOURCE);
            Assert.IsNotNull(configAsset);
            OpenNoFileMessageBoxConfig = JsonUtility.FromJson<MessageBoxConfig>(configAsset.text);
        }

        public void SelectEntry(OpenCircuitScrollViewItem selectedEntry)
        {
            foreach (var entry in Entries)
            {
                entry.gameObject.GetComponent<Image>().color = new Color32(204, 204, 204, 255);
                entry.Text.GetComponent<Text>().color = new Color32(50, 50, 50, 255);
            }
            CurrentlySelectedEntry = selectedEntry;
            CurrentlySelectedEntry.gameObject.GetComponent<Image>().color = new Color32(135, 135, 135, 135);
            CurrentlySelectedEntry.Text.GetComponent<Text>().color = new Color32(255, 255, 255, 255);
        }
    }
}
