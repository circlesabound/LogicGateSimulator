using System;
using System.Collections.Generic;

namespace Assets.Scripts.Savefile
{
    [Serializable]
    public class CircuitConfig
    {
        public List<LogicComponentConfig> logic_components;
        public List<EdgeConfig> edges;

        public CircuitConfig(List<LogicComponentConfig> componentConfigs, List<EdgeConfig> edgeConfigs)
        {
            logic_components = componentConfigs;
            edges = edgeConfigs;
        }
    }
}
