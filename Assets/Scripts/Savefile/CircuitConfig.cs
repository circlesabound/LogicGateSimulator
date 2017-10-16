using System;
using System.Collections.Generic;

namespace Assets.Scripts.Savefile
{
    [Serializable]
    public class CircuitConfig
    {
        public List<LogicComponentConfig> logic_components;
        public List<EdgeConfig> edges;
        public List<InputToggleConfig> toggles;

        public CircuitConfig(List<LogicComponentConfig> componentConfigs, List<EdgeConfig> edgeConfigs, List<InputToggleConfig> toggleConfigs)
        {
            logic_components = componentConfigs;
            edges = edgeConfigs;
            toggles = toggleConfigs;
        }
    }
}
