using Assets.Scripts.ScratchPad;
using Assets.Scripts.UI.MessageBoxes;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;

namespace Assets.Scripts.UI
{
    public class UIOverlayMenuNewButton : MonoBehaviour, IMessageBoxTriggerTarget, IInfoPanelTextProvider, IPointerEnterHandler, IPointerExitHandler
    {
        private const string NEW_BUTTON_INFO_PANEL_TITLE = "New circuit";
        private const string NEW_BUTTON_INFO_PANEL_DESCRIPTION = "Create a new circuit.";
        private const string UNSAVED_CHANGES_MESSAGE_BOX_CONFIG_RESOURCE = "Configs/MessageBoxes/unsaved_changes";
        private SPCanvas Canvas;
        private UIMessageBoxFactory MessageBoxFactory;
        private MessageBoxConfig UnsavedChangesMessageBoxConfig;

        private UIOverlayInfoPanel InfoPanel;

        public string InfoPanelTitle
        {
            get
            {
                return NEW_BUTTON_INFO_PANEL_TITLE;
            }
        }

        public string InfoPanelText
        {
            get
            {
                return NEW_BUTTON_INFO_PANEL_DESCRIPTION;
            }
        }

        public void OnButtonClick()
        {
            // Check for unsaved changes
            if (Canvas.IsUnsaved)
            {
                Canvas.Frozen = true;
                MessageBoxFactory.MakeFromConfig(UnsavedChangesMessageBoxConfig, this);
            }
            else
            {
                ClearCanvas();
                Canvas.CurrentMode = GameMode.Sandbox;
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
            Canvas.SetAsSaved();
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
