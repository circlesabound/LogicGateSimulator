using System;

namespace Assets.Scripts.UI.MessageBoxes
{
    [Serializable]
    public class MessageBoxConfig
    {
        public string title;
        public string message;
        public MessageBoxScrollView scroll_view;
        public MessageBoxTextInputConfig text_input;
        public MessageBoxNumberSliderConfig number_slider;
        public MessageBoxButtonContainer buttons;
        public float x_position;
        public float y_position;
        public bool is_modal;
        public string classname;

        [Serializable]
        public class MessageBoxScrollView
        {
            public bool hidden;
            public string content_type;
            public MessageBoxScrollViewStaticContent static_content;
            public MessageBoxScrollViewDynamicContent dynamic_content;
        }

        [Serializable]
        public class MessageBoxScrollViewStaticContent
        {
            public MessageBoxScrollViewItemConfig[] items;
        }

        [Serializable]
        public class MessageBoxScrollViewDynamicContent
        {
            public MessageBoxScrollViewItemConfig item_blueprint;
        }

        [Serializable]
        public class MessageBoxScrollViewItemConfig
        {
            public string label;
            public string classname;
            public string onclick;
        }

        [Serializable]
        public class MessageBoxTextInputConfig
        {
            public bool hidden;
            public string placeholder;
        }

        [Serializable]
        public class MessageBoxNumberSliderConfig
        {
            public bool hidden;
            public float min_value;
            public float max_value;
            public bool use_integer_values;
            public MessageBoxNumberSliderLabelConfig label_left;
            public MessageBoxNumberSliderLabelConfig label_right;
        }

        [Serializable]
        public class MessageBoxNumberSliderLabelConfig
        {
            public bool hidden;
            public string content_type;
            public MessageBoxNumberSliderStaticLabel static_content;
            public MessageBoxNumberSliderDynamicLabel dynamic_provider;
        }

        [Serializable]
        public class MessageBoxNumberSliderStaticLabel
        {
            public string label;
        }

        [Serializable]
        public class MessageBoxNumberSliderDynamicLabel
        {
            public string classname;
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
