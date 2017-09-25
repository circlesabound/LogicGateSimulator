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
            LogicComponent true_constant = new TrueConstant();
            LogicComponent false_constant = new FalseConstant();

            // Check default behaviour is sane:
            Assert.AreEqual(and_gate.Simulate(), new List<bool> { false });
            and_gate.AddInput(0, true_constant, 0);
            Assert.AreEqual(and_gate.Simulate(), new List<bool> { false });
            and_gate.RemoveInput(0);
            Assert.AreEqual(and_gate.Simulate(), new List<bool> { false });

            TestUtil.Test_TruthTable(and_gate, new bool[2, 2] { { false, false }, { false, true } });
        }

        [Test]
        public void Test_Connection()
        {
            LogicComponent connection = new Connection();
            LogicComponent true_constant = new TrueConstant();
            LogicComponent false_constant = new FalseConstant();

            // Check default behaviour is sane:
            Assert.AreEqual(connection.Simulate(), new List<bool> { false });

            // Check truth table:
            connection.AddInput(0, true_constant, 0);
            Assert.AreEqual(connection.Simulate(), new List<bool> { true });
            connection.RemoveInput(0);
            connection.AddInput(0, false_constant, 0);
            Assert.AreEqual(connection.Simulate(), new List<bool> { false });
        }

        [Test]
        public void Test_ConstantComponents()
        {
            LogicComponent true_constant = new TrueConstant();
            Assert.AreEqual(true_constant.Outputs, new List<bool>() { true });
            Assert.AreEqual(true_constant.Simulate(), new List<bool> { true });

            LogicComponent false_constant = new FalseConstant();
            Assert.AreEqual(false_constant.Outputs, new List<bool>() { false });
            Assert.AreEqual(false_constant.Simulate(), new List<bool> { false });
        }

        [Test]
        public void Test_InputComponent()
        {
            InputComponent input_component = new InputComponent();

            input_component.SetValue(true);
            Assert.AreEqual(input_component.Simulate(), new List<bool> { true });
            input_component.SetValue(false);
            Assert.AreEqual(input_component.Simulate(), new List<bool> { false });
        }

        [Test]
        public void Test_NotGateSimulate()
        {
            LogicComponent not_gate = new NotGate();
            LogicComponent true_constant = new TrueConstant();
            LogicComponent false_constant = new FalseConstant();

            // Check default behaviour is sane:
            Assert.AreEqual(not_gate.Simulate(), new List<bool> { true });

            // Check truth table:
            not_gate.AddInput(0, true_constant, 0);
            Assert.AreEqual(not_gate.Simulate(), new List<bool> { false });
            not_gate.RemoveInput(0);
            not_gate.AddInput(0, false_constant, 0);
            Assert.AreEqual(not_gate.Simulate(), new List<bool> { true });
        }

        [Test]
        public void Test_OrGateSimulate()
        {
            LogicComponent or_gate = new OrGate();
            LogicComponent true_constant = new TrueConstant();
            LogicComponent false_constant = new FalseConstant();

            // Check default behaviour and behaviour with only 1 input is sane:
            Assert.AreEqual(or_gate.Simulate(), new List<bool> { false });
            or_gate.AddInput(0, false_constant, 0);
            Assert.AreEqual(or_gate.Simulate(), new List<bool> { false });
            or_gate.RemoveInput(0);
            or_gate.AddInput(0, true_constant, 0);
            Assert.AreEqual(or_gate.Simulate(), new List<bool> { true });
            or_gate.RemoveInput(0);
            Assert.AreEqual(or_gate.Simulate(), new List<bool> { false });

            TestUtil.Test_TruthTable(or_gate, new bool[2, 2] { { false, true }, { true, true } });
        }

        [Test]
        public void Test_XorGateSimulate()
        {
            LogicComponent xor_gate = new XorGate();
            LogicComponent true_constant = new TrueConstant();
            LogicComponent false_constant = new FalseConstant();

            // Check default behaviour and behaviour with only 1 input is sane:
            Assert.AreEqual(xor_gate.Simulate(), new List<bool> { false });
            xor_gate.AddInput(0, false_constant, 0);
            Assert.AreEqual(xor_gate.Simulate(), new List<bool> { false });
            xor_gate.RemoveInput(0);
            xor_gate.AddInput(0, true_constant, 0);
            Assert.AreEqual(xor_gate.Simulate(), new List<bool> { true });
            xor_gate.RemoveInput(0);
            Assert.AreEqual(xor_gate.Simulate(), new List<bool> { false });

            TestUtil.Test_TruthTable(xor_gate, new bool[2, 2] { { false, true }, { true, false } });
        }
    }
}
