using System;

namespace Assets.Scripts.UI.MessageBoxes
{
    [Serializable]
    public class MessageBoxConfig
    {
        public string title;
        public string message;
        public MessageBoxTextInputConfig text_input;
        public MessageBoxButtonContainer buttons;
        public bool is_modal;
        public string classname;

        [Serializable]
        public class MessageBoxTextInputConfig
        {
            public bool hidden;
            public string placeholder;
        }

        [Serializable]
        public class MessageBoxButtonContainer
        {
            public bool hidden;
            public MessageBoxButton positive_button;
            public MessageBoxButton neutral_button;
            public MessageBoxButton negative_button;
        }

        [Serializable]
        public class MessageBoxButton
        {
            public bool hidden;
            public string label;
            public string onclick;
        }
    }
}
