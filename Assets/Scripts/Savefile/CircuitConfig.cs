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
        public List<ClockComponentConfig> clocks;
        public bool is_challenge;

        public CircuitConfig(List<LogicComponentConfig> componentConfigs, List<EdgeConfig> edgeConfigs, List<InputToggleConfig> toggleConfigs, List<ClockComponentConfig> clockConfigs, bool isChallenge = false)
        {
            logic_components = componentConfigs;
            edges = edgeConfigs;
            toggles = toggleConfigs;
            clocks = clockConfigs;
            is_challenge = isChallenge;
        }
    }
}
