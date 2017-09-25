using NUnit.Framework;
using System.Collections.Generic;

namespace Assets.Editor.Tests
{
    internal static class TestUtil
    {
        /// <summary>
        /// Tests whether a component has a specified truth table.
        /// </summary>
        /// <param name="component">The component to test</param>
        /// <param name="truth_table">
        /// A 2*2 truth table, where truth_table[f][s] is the expected value
        /// when the first input is f and the second input is s.</param>
        public static void Test_TruthTable(LogicComponent component, bool[,] truth_table)
        {
            LogicComponent[] constant_components = new LogicComponent[] { new FalseConstant(), new TrueConstant() };
            for (int f = 0; f < 2; f++)
            {
                for (int s = 0; s < 2; s++)
                {
                    component.RemoveInput(0);
                    component.RemoveInput(1);
                    component.AddInput(0, constant_components[f], 0);
                    component.AddInput(1, constant_components[s], 0);
                    Assert.AreEqual(component.Simulate(), new List<bool>() { truth_table[f, s] },
                                    "Failed to simulate component with inputs: " + f + " and " + s);
                }
            }
        }
    }
}
