using NUnit.Framework;
using System.Collections.Generic;

namespace Assets.Editor.Tests
{
    internal class CircuitTests
    {
        [Test]
        public void Test_AddAndRemoveComponentsToCircuit()
        {
            Circuit circuit = new Circuit();
            TrueConstant true_constant = new TrueConstant();
            Assert.IsTrue(circuit.AddComponent(true_constant));
            // Check we can't add the same component twice:
            Assert.IsFalse(circuit.AddComponent(true_constant));
            // Try removing:
            Assert.IsTrue(circuit.RemoveComponent(true_constant));
            Assert.IsFalse(circuit.RemoveComponent(true_constant));
            // Try re-adding again:
            Assert.IsTrue(circuit.AddComponent(true_constant));
        }

        [Test]
        public void Test_CircuitSimulate_SelfCycle()
        {
            LogicComponent not_gate = new NotGate();
            LogicComponent connection = new Connection();
            Circuit circuit = new Circuit();

            /* The circuit is just a not gate whose output is connected to its own input by a wire */
            connection.AddInput(0, not_gate, 0);
            not_gate.AddInput(0, connection, 0);
            Assert.IsTrue(circuit.AddComponent(not_gate));
            Assert.IsTrue(circuit.AddComponent(connection));

            /* The circuit's state should repeat every 4 simulate steps.
             * The expected result of each of the 4 simulate steps in order:
             * 1) The output of the not gate switches to True.
             * 2) The output of the connection switches to True.
             * 3) The output of the not gate switches back to False.
             * 4) The output of connection switches back to False.
             */
            for (int i = 0; i < 100; i++)
            {
                // After first step, not gate should have true output:
                Assert.IsTrue(circuit.Simulate());
                Assert.AreEqual(not_gate.Outputs, new List<bool>() { true });
                // After second step, connection should have true output too:
                Assert.IsTrue(circuit.Simulate());
                Assert.AreEqual(connection.Outputs, new List<bool>() { true });
                // After third step, not gate should have false output again:
                Assert.IsTrue(circuit.Simulate());
                Assert.AreEqual(not_gate.Outputs, new List<bool>() { false });
                // After fourth step, connection should have false output again:
                Assert.IsTrue(circuit.Simulate());
                Assert.AreEqual(connection.Outputs, new List<bool>() { false });
            }
        }

        [Test]
        public void Test_CircuitSimulate_SmallTree()
        {
            LogicComponent true_constant1 = new TrueConstant();
            LogicComponent true_constant2 = new TrueConstant();
            LogicComponent false_constant = new FalseConstant();
            LogicComponent and_gate = new AndGate();
            LogicComponent or_gate = new OrGate();

            LogicComponent true_constant1_connection = new Connection();
            LogicComponent true_constant2_connection = new Connection();
            LogicComponent false_constant_connection = new Connection();
            LogicComponent and_gate_connection = new Connection();

            Circuit circuit = new Circuit();

            /* Test will be 2 TRUEs connected to an AND gate.
             * This AND gate and a FALSE will then be connected to the OR gate.
             */
            true_constant1_connection.AddInput(0, true_constant1, 0);
            true_constant2_connection.AddInput(0, true_constant2, 0);
            and_gate.AddInput(0, true_constant1_connection, 0);
            and_gate.AddInput(1, true_constant2_connection, 0);
            and_gate_connection.AddInput(0, and_gate, 0);
            false_constant_connection.AddInput(0, false_constant, 0);
            or_gate.AddInput(0, and_gate_connection, 0);
            or_gate.AddInput(1, false_constant_connection, 0);

            Assert.IsTrue(circuit.AddComponent(true_constant1));
            Assert.IsTrue(circuit.AddComponent(true_constant2));
            Assert.IsTrue(circuit.AddComponent(false_constant));
            Assert.IsTrue(circuit.AddComponent(and_gate));
            Assert.IsTrue(circuit.AddComponent(or_gate));
            Assert.IsTrue(circuit.AddComponent(true_constant1_connection));
            Assert.IsTrue(circuit.AddComponent(true_constant2_connection));
            Assert.IsTrue(circuit.AddComponent(false_constant_connection));
            Assert.IsTrue(circuit.AddComponent(and_gate_connection));

            // Try first step: the output of the connections from the constants should change.
            Assert.IsTrue(circuit.Simulate());
            Assert.AreEqual(true_constant1_connection.Outputs, new List<bool>() { true });
            Assert.AreEqual(true_constant2_connection.Outputs, new List<bool>() { true });
            // Try second step: the output from the and gate should change.
            Assert.IsTrue(circuit.Simulate());
            Assert.AreEqual(and_gate.Outputs, new List<bool>() { true });
            // Try third step: the output of the connections from the and gate should change.
            Assert.IsTrue(circuit.Simulate());
            Assert.AreEqual(and_gate_connection.Outputs, new List<bool>() { true });
            // Try fourth step: the output of the or gate should change.
            Assert.IsTrue(circuit.Simulate());
            Assert.AreEqual(or_gate.Outputs, new List<bool>() { true });

            // Ensure the state has stabilized into the expected state:
            for (int i = 0; i < 100; i++)
            {
                Assert.IsTrue(circuit.Simulate());
                Assert.AreEqual(true_constant1_connection.Outputs, new List<bool>() { true });
                Assert.AreEqual(true_constant2_connection.Outputs, new List<bool>() { true });
                Assert.AreEqual(and_gate.Outputs, new List<bool>() { true });
                Assert.AreEqual(and_gate_connection.Outputs, new List<bool>() { true });
                Assert.AreEqual(or_gate.Outputs, new List<bool>() { true });
            }
        }
    }
}
