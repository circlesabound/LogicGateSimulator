using static Assets.Scripts.UI.MessageBoxes.UIMessageBox;

namespace Assets.Scripts.UI.MessageBoxes
{
    public class MessageBoxTriggerData
    {
        public UIMessageBox Sender;
        public string TextInput;
        public float? NumberInput;
        public MessageBoxButtonType? ButtonPressed;
    }
}
