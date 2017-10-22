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
        public List<TestCaseConfig> test_cases;
        public GameMode game_mode;

        public CircuitConfig(List<LogicComponentConfig> componentConfigs, List<EdgeConfig> edgeConfigs, List<InputToggleConfig> toggleConfigs, List<InputToggleConfig> numbered_toggleConfigs, List<ClockComponentConfig> clockConfigs, List<TestCaseConfig> testCaseConfigs, GameMode mode)
        {
            logic_components = componentConfigs;
            edges = edgeConfigs;
            toggles = toggleConfigs;
            numbered_toggles = numbered_toggleConfigs;
            clocks = clockConfigs;
            test_cases = testCaseConfigs;
            game_mode = mode;
        }
    }
}
