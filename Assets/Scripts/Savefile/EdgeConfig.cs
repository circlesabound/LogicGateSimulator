using System;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Scripts.Savefile
{
    [Serializable]
    public class EdgeConfig
    {
        public string[] component_guid_strings;
        public int[] connector_ids;

        public EdgeConfig(Guid componentGuid0, int connectorId0, Guid componentGuid1, int connectorId1)
            : this(componentGuid0.ToString(), connectorId0, componentGuid1.ToString(), connectorId1)
        {
        }

        public EdgeConfig(string componentGuidString0, int connectorId0, string componentGuidString1, int connectorId1)
        {
            this.component_guid_strings = new string[2] { componentGuidString0, componentGuidString1 };
            this.connector_ids = new int[2] { connectorId0, connectorId1 };
        }

        // not serialised
        public Guid[] ComponentGuids
        {
            get
            {
                return new[] {
                    Guid.Parse(component_guid_strings[0]),
                    Guid.Parse(component_guid_strings[1])
                };
            }
        }
    }
}
