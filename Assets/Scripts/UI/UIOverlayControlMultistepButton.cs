using Assets.Scripts.ScratchPad;
using Assets.Scripts.UI.MessageBoxes;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class UIOverlayControlMultistepButton : MonoBehaviour, IMessageBoxTriggerTarget
    {
        private SPCanvas Canvas;
        private UIMessageBoxFactory MessageBoxFactory;

        private const string SET_MULTISTEP_MESSAGE_BOX_CONFIG_RESOURCE = "Configs/MessageBoxes/set_multistep";
        private MessageBoxConfig SetMultistepMessageBoxConfig;

        /// <summary>
        /// Creates window for setting how many steps to run.
        /// Linked to button script in Unity inspector
        /// </summary>
        public void OnButtonClick()
        {
            Canvas.Frozen = true;
            MessageBoxFactory.MakeFromConfig(SetMultistepMessageBoxConfig, this);
        }

        /// <summary>
        /// Callback to be executed from SetMultistepMessageBox.
        /// Unfreezes the canvas and closes the message box.
        /// Positive trigger causes Circuit to run for a limited number of frames.
        /// </summary>
        /// <param name="triggerData">Data to pass from the trigger source to the trigger target.</param>
        public void Trigger(MessageBoxTriggerData triggerData)
        {
            if (triggerData.ButtonPressed == UIMessageBox.MessageBoxButtonType.Positive)
            {
                Assert.IsTrue(triggerData.NumberInput.HasValue);
                int StepsToRun = (int) triggerData.NumberInput.Value;
                Canvas.RunForKSteps(StepsToRun);
            }

            Canvas.Frozen = false;
            Destroy(triggerData.Sender.gameObject);
        }

        private void Awake()
        {
            MessageBoxFactory = new UIMessageBoxFactory();

            // Load the message box config for setting the internal clock
            TextAsset configAsset = Resources.Load<TextAsset>(SET_MULTISTEP_MESSAGE_BOX_CONFIG_RESOURCE);
            Assert.IsNotNull(configAsset);
            SetMultistepMessageBoxConfig = JsonUtility.FromJson<MessageBoxConfig>(configAsset.text);
        }

        private void Start()
        {
            Canvas = FindObjectOfType<SPCanvas>();
            Assert.IsNotNull(Canvas);
        }

        private void Update()
        {
            //
        }
    }
}
