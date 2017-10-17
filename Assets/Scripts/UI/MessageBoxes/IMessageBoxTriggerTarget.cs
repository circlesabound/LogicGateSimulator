namespace Assets.Scripts.UI.MessageBoxes
{
    public interface IMessageBoxTriggerTarget
    {
        /// <summary>
        /// A callback to be executed by a message box.
        /// </summary>
        /// <param name="triggerData">Data to pass from the message box to the trigger target</param>
        void Trigger(MessageBoxTriggerData triggerData);
    }
}
