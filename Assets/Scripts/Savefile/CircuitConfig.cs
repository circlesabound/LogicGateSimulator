using Assets.Scripts.ScratchPad;
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
        public List<InputToggleConfig> numbered_toggles;
        public List<ClockComponentConfig> clocks;
        public GameMode game_mode;

        public CircuitConfig(List<LogicComponentConfig> componentConfigs, List<EdgeConfig> edgeConfigs, List<InputToggleConfig> toggleConfigs, List<InputToggleConfig> numbered_toggleConfigs, List<ClockComponentConfig> clockConfigs, GameMode mode)
        {
            logic_components = componentConfigs;
            edges = edgeConfigs;
            toggles = toggleConfigs;
            numbered_toggles = numbered_toggleConfigs;
            clocks = clockConfigs;
            game_mode = mode;
        }
    }
}
