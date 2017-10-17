using NUnit.Framework;
using System;
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
        public void Test_Splitter()
        {
            LogicComponent fanout = new Splitter();

            // Check truth table:
            Assert.AreEqual(fanout.Simulate(new [] { true }), new List<bool> { true, true });
            Assert.AreEqual(fanout.Simulate(new [] { false }), new List<bool> { false, false });
        }

        [Test]
        public void Test_ConstantComponents()
        {
            LogicComponent true_constant = new TrueConst();
            Assert.AreEqual(true_constant.Outputs, new List<bool>() { true });
            Assert.AreEqual(true_constant.Simulate(new bool[] {}), new List<bool> { true });

            LogicComponent false_constant = new FalseConst();
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

        [Test]
        public void Test_OutputSimulate()
        {
            Output output = new Output();

            Assert.AreEqual(output.Value, false);
            output.Simulate(new bool[] { true });
            Assert.AreEqual(output.Value, true);
            output.Simulate(new bool[] { false });
            Assert.AreEqual(output.Value, false);
        }

        [Test]
        public void Test_InputOutputSanityCheck()
        {
            LogicComponent xor_gate = new XorGate();

            // Assert input constraints are checked
            Assert.That(() => xor_gate.Simulate(new [] { true }), Throws.ArgumentException);
            Assert.That(() => xor_gate.Simulate(new [] { true, false }), Throws.Nothing);
            Assert.That(() => xor_gate.Simulate(new [] { true, false, true }),
                Throws.ArgumentException);

            // Assert output constraints are checked
            Assert.That(() => xor_gate.Outputs = new List<bool> { }, Throws.ArgumentException);
            Assert.That(() => xor_gate.Outputs = new List<bool> { true }, Throws.Nothing);
            Assert.That(() => xor_gate.Outputs = new List<bool> { true, false },
                Throws.ArgumentException);
        }

        [Test]
        public void Test_Clock()
        {
            // Assert that clock with zero period is invalid
            Assert.That(() => new Clock(0), Throws.TypeOf<ArgumentOutOfRangeException>());

            // Check outputs for different periods
            for (uint i = 1; i < 100; i++)
            {
                Circuit circuit = new Circuit();
                LogicComponent clock = new Clock(i);
                circuit.AddComponent(clock);
                for (uint j = 0; j < 400; j++)
                {
                    bool expected = ((j / i) % 2 != 0);
                    Assert.AreEqual(clock.Outputs[0], expected);
                    circuit.Simulate();
                }
            }
        }
    }
}
