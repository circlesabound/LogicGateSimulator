using System;

namespace Assets.Scripts.UI.MessageBoxes
{
    public class ConfirmMessageBox : UIMessageBox
    {
        public void OnCancelButtonClick()
        {
            if (TriggerTarget != null)
            {
                MessageBoxTriggerData triggerData = new MessageBoxTriggerData
                {
                    ButtonPressed = MessageBoxButtonType.Negative,
                    Sender = this
                };
                TriggerTarget.Trigger(triggerData);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void OnConfirmButtonClick()
        {
            if (TriggerTarget != null)
            {
                MessageBoxTriggerData triggerData = new MessageBoxTriggerData
                {
                    ButtonPressed = MessageBoxButtonType.Positive,
                    Sender = this
                };
                TriggerTarget.Trigger(triggerData);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public override void Trigger(MessageBoxTriggerData triggerData)
        {
            // shouldn't be accepting triggers
            throw new InvalidOperationException();
        }
    }
}
