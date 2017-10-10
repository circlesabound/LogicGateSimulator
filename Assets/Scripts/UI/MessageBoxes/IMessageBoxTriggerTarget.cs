using UnityEngine.EventSystems;

namespace Assets.Scripts.UI.MessageBoxes
{
    public interface IMessageBoxTriggerTarget
    {
        void Trigger(MessageBoxTriggerData triggerData);
    }
}
