using Assets.Scripts.ScratchPad;
using Assets.Scripts.UI.MessageBoxes;
using UnityEngine;
using UnityEngine.Assertions;

namespace Assets.Scripts.UI
{
    public class UIOverlayMenuNewButton : MonoBehaviour, IMessageBoxTriggerTarget
    {
        private const string UNSAVED_CHANGES_MESSAGE_BOX_CONFIG_RESOURCE = "Configs/MessageBoxes/unsaved_changes";
        private SPCanvas Canvas;
        private UIMessageBoxFactory MessageBoxFactory;
        private MessageBoxConfig UnsavedChangesMessageBoxConfig;

        public void OnButtonClick()
        {
            // Check for unsaved changes
            if (Canvas.LastSavedComponentsHash != Canvas.ComponentsHash)
            {
                Canvas.Frozen = true;
                MessageBoxFactory.MakeFromConfig(UnsavedChangesMessageBoxConfig, this);
            }
            else
            {
                ClearCanvas();
            }
        }

        public void Trigger(MessageBoxTriggerData triggerData)
        {
            // Accept trigger from unsaved changes confirmation box
            if (triggerData.Sender.GetType() == typeof(ConfirmMessageBox))
            {
                if (triggerData.ButtonPressed == UIMessageBox.MessageBoxButtonType.Positive)
                {
                    ClearCanvas();
                }
            }
            Canvas.Frozen = false;
            Destroy(triggerData.Sender.gameObject);
        }

        private void Awake()
        {
            MessageBoxFactory = new UIMessageBoxFactory();

            // Load the message box config for unsaved changes
            var configAsset = Resources.Load<TextAsset>(UNSAVED_CHANGES_MESSAGE_BOX_CONFIG_RESOURCE);
            Assert.IsNotNull(configAsset);
            UnsavedChangesMessageBoxConfig = JsonUtility.FromJson<MessageBoxConfig>(configAsset.text);
        }

        private void ClearCanvas()
        {
            for (int i = Canvas.Components.Count - 1; i >= 0; --i)
            {
                Canvas.Components[i].Delete();
            }
            // Reset savefile hash
            Canvas.LastSavedComponentsHash = Canvas.ComponentsHash;
        }

        private void Start()
        {
            Canvas = FindObjectOfType<SPCanvas>();
            Assert.IsNotNull(Canvas);
        }
    }
}
