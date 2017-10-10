using System;
using System.Collections.Generic;

namespace Assets.Scripts.Savefile
{
    [Serializable]
    public class ScratchPadConfig
    {
        public List<LogicComponentConfig> logic_components;
        public List<EdgeConfig> edges;

        public ScratchPadConfig(List<LogicComponentConfig> componentConfigs, List<EdgeConfig> edgeConfigs)
        {
            logic_components = componentConfigs;
            edges = edgeConfigs;
        }
    }
}
