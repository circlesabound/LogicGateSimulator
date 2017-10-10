using Assets.Scripts.Util;
using UnityEngine;

namespace Assets.Scripts.UI.MessageBoxes
{
    public abstract class UIMessageBox : MonoBehaviour, IMessageBoxTriggerTarget
    {
        public IMessageBoxTriggerTarget TriggerTarget;

        public enum MessageBoxButtonType
        {
            Positive,
            Neutral,
            Negative
        }

        public GameObject BackgroundShade
        {
            get
            {
                return gameObject.FindChildGameObject("UIMessageBoxBackgroundShade");
            }
        }

        public GameObject Message
        {
            get
            {
                return gameObject.FindChildGameObject("UIMessageBox/UIMessageBoxMessage");
            }
        }

        public GameObject MessageBox
        {
            get
            {
                return gameObject.FindChildGameObject("UIMessageBox");
            }
        }

        public GameObject NegativeButton
        {
            get
            {
                return gameObject.FindChildGameObject("UIMessageBox/UIMessageBoxButtonContainer/UIMessageBoxNegativeButton");
            }
        }

        public GameObject NeutralButton
        {
            get
            {
                return gameObject.FindChildGameObject("UIMessageBox/UIMessageBoxButtonContainer/UIMessageBoxNeutralButton");
            }
        }

        public GameObject PositiveButton
        {
            get
            {
                return gameObject.FindChildGameObject("UIMessageBox/UIMessageBoxButtonContainer/UIMessageBoxPositiveButton");
            }
        }

        public GameObject TextInput
        {
            get
            {
                return gameObject.FindChildGameObject("UIMessageBox/UIMessageBoxTextInput");
            }
        }

        public GameObject Title
        {
            get
            {
                return gameObject.FindChildGameObject("UIMessageBox/UIMessageBoxTitle");
            }
        }

        public abstract void Trigger(MessageBoxTriggerData triggerData);
    }
}
