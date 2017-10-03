using System;
using System.Collections.Generic;

namespace Assets.Scripts.UI
{
    [Serializable]
    public class UIToolboxPanelConfig
    {
        public string panel_name;
        public string sprite_selected;
        public string sprite_unselected;
        public List<UIToolboxComponentConfig> components;
    }
}
