using Assets.Scripts.ScratchPad;
using Assets.Scripts.Util;
using System;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace Assets.Scripts.UI.MessageBoxes
{
    public class SetNumberOfStepsMessageBox : UIMessageBox
    {
        private float SliderValue
        {
            get
            {
                return SliderContainer.FindChildGameObject("UIMessageBoxNumberSlider").GetComponent<Slider>().value;
            }
            set
            {
                SliderContainer.FindChildGameObject("UIMessageBoxNumberSlider").GetComponent<Slider>().value = value;
            }
        }

        public void OnCancelButtonClick()
        {
            var triggerData = new MessageBoxTriggerData
            {
                ButtonPressed = MessageBoxButtonType.Negative,
                Sender = this,
            };
            TriggerTarget.Trigger(triggerData);
        }

        public void OnConfirmButtonClick()
        {
            var triggerData = new MessageBoxTriggerData
            {
                ButtonPressed = MessageBoxButtonType.Positive,
                Sender = this,
                NumberInput = SliderValue
            };
            TriggerTarget.Trigger(triggerData);
        }

        public override void Trigger(MessageBoxTriggerData triggerData)
        {
            // shouldn't be accepting triggers
            throw new NotImplementedException();
        }

        private void Start()
        {
            SliderValue = 1;
        }
    }
}
