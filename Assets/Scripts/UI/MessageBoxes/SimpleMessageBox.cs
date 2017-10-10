using System;

namespace Assets.Scripts.UI.MessageBoxes
{
    public class SimpleMessageBox : UIMessageBox
    {
        public void OnOkayButtonClick()
        {
            if (TriggerTarget != null)
            {
                MessageBoxTriggerData triggerData = new MessageBoxTriggerData
                {
                    ButtonPressed = MessageBoxButtonType.Neutral,
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
