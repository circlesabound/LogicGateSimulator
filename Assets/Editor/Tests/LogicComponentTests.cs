using NUnit.Framework;
using System.Collections.Generic;

namespace Assets.Editor.Tests
{
    internal class LogicComponentTests
    {
        [Test]
        public void Test_AndGateSimulate()
        {
            LogicComponent and_gate = new AndGate();

            TestUtil.Test_TruthTable(and_gate, new bool[2, 2] { { false, false }, { false, true } });
        }

        [Test]
        public void Test_Connection()
        {
            LogicComponent connection = new Connection();

            // Check truth table:
            Assert.AreEqual(connection.Simulate(new [] { true }), new List<bool> { true });
            Assert.AreEqual(connection.Simulate(new [] { false }), new List<bool> { false });
        }

        [Test]
        public void Test_ConstantComponents()
        {
            LogicComponent true_constant = new TrueConstant();
            Assert.AreEqual(true_constant.Outputs, new List<bool>() { true });
            Assert.AreEqual(true_constant.Simulate(new bool[] {}), new List<bool> { true });

            LogicComponent false_constant = new FalseConstant();
            Assert.AreEqual(false_constant.Outputs, new List<bool>() { false });
            Assert.AreEqual(false_constant.Simulate(new bool[] {}), new List<bool> { false });
        }

        [Test]
        public void Test_InputComponent()
        {
            InputComponent input_component = new InputComponent();

            input_component.SetValue(true);
            Assert.AreEqual(input_component.Simulate(new bool[] {}), new List<bool> { true });
            input_component.SetValue(false);
            Assert.AreEqual(input_component.Simulate(new bool[] {}), new List<bool> { false });
        }

        [Test]
        public void Test_NotGateSimulate()
        {
            LogicComponent not_gate = new NotGate();

            // Check truth table:
            Assert.AreEqual(not_gate.Simulate(new [] { true }), new List<bool> { false });
            Assert.AreEqual(not_gate.Simulate(new [] { false }), new List<bool> { true });
        }

        [Test]
        public void Test_OrGateSimulate()
        {
            LogicComponent or_gate = new OrGate();

            TestUtil.Test_TruthTable(or_gate, new bool[2, 2] { { false, true }, { true, true } });
        }

        [Test]
        public void Test_XorGateSimulate()
        {
            LogicComponent xor_gate = new XorGate();

            TestUtil.Test_TruthTable(xor_gate, new bool[2, 2] { { false, true }, { true, false } });
        }
    }
}
