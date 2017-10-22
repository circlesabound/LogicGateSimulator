using Assets.Scripts.ScratchPad;
using Assets.Scripts.UI.MessageBoxes;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;

namespace Assets.Scripts.UI
{
    public class UIOverlayMenuInternalClockSpeedButton : MonoBehaviour, IMessageBoxTriggerTarget, IInfoPanelTextProvider, IPointerEnterHandler, IPointerExitHandler
    {
        private SPCanvas Canvas;
        private UIMessageBoxFactory MessageBoxFactory;

        private UIOverlayInfoPanel InfoPanel;

        public float FramesPerSecond
        {
            get;
            private set;
        }

        public string InfoPanelTitle
        {
            get
            {
                return SETTINGS_BUTTON_INFO_PANEL_TITLE;
            }
        }

        public string InfoPanelText
        {
            get
            {
                return SETTINGS_BUTTON_INFO_PANEL_DESCRIPTION;
            }
        }

        private const string SETTINGS_BUTTON_INFO_PANEL_TITLE = "Settings";
        private const string SETTINGS_BUTTON_INFO_PANEL_DESCRIPTION = "Configure application settings.";

        private const string INTERNAL_CLOCK_MESSAGE_BOX_CONFIG_RESOURCE = "Configs/MessageBoxes/internal_clock";
        private MessageBoxConfig InternalClockMessageBoxConfig;

        /// <summary>
        /// Linked to button click in Unity inspector
        /// </summary>
        public void OnButtonClick()
        {
            Canvas.Frozen = true;
            MessageBoxFactory.MakeFromConfig(InternalClockMessageBoxConfig, this);
        }

        /// <summary>
        /// Callback to be executed from InternalClockMessageBox.
        /// Unfreezes the canvas and closes the message box.
        /// Positive trigger changes the frame rate at which the Canvas updates.
        /// </summary>
        /// <param name="triggerData">Data to pass from the trigger source to the trigger target.</param>
        public void Trigger(MessageBoxTriggerData triggerData)
        {
            if (triggerData.ButtonPressed == UIMessageBox.MessageBoxButtonType.Positive)
            {
                Assert.IsTrue(triggerData.NumberInput.HasValue);
                FramesPerSecond = triggerData.NumberInput.Value;
                // Shouldn't be possible at all so we'll assert it out.
                Assert.IsTrue(FramesPerSecond > 0);

                float SecondsPerFrame = 1 / FramesPerSecond;

                Canvas.SecondsPerUpdate = SecondsPerFrame;
            }

            Canvas.Frozen = false;
            Destroy(triggerData.Sender.gameObject);
        }

        private void Awake()
        {
            MessageBoxFactory = new UIMessageBoxFactory();

            // Load the message box config for setting the internal clock
            TextAsset configAsset = Resources.Load<TextAsset>(INTERNAL_CLOCK_MESSAGE_BOX_CONFIG_RESOURCE);
            Assert.IsNotNull(configAsset);
            InternalClockMessageBoxConfig = JsonUtility.FromJson<MessageBoxConfig>(configAsset.text);
        }

        private void Start()
        {
            Canvas = FindObjectOfType<SPCanvas>();
            Assert.IsNotNull(Canvas);
            InfoPanel = FindObjectOfType<UIOverlayInfoPanel>();
            Assert.IsNotNull(InfoPanel);
            FramesPerSecond = 1 / Canvas.SecondsPerUpdate;
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
