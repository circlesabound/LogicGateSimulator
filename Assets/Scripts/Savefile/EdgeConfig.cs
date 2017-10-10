using System;

namespace Assets.Scripts.Savefile
{
    [Serializable]
    public class EdgeConfig
    {
        public string[] component_guid_strings;

        public EdgeConfig(Guid componentGuid0, Guid componentGuid1)
            : this(componentGuid0.ToString(), componentGuid1.ToString())
        {
        }

        public EdgeConfig(string componentGuidString0, string componentGuidString1)
        {
            this.component_guid_strings = new string[2] { componentGuidString0, componentGuidString1 };
        }
    }
}
