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
            TrueConst true_constant = new TrueConst();
            Assert.That(() => circuit.AddComponent(true_constant), Throws.Nothing);
            // Check we can't add the same component twice:
            Assert.That(() => circuit.AddComponent(true_constant), Throws.ArgumentException);
            // Try removing:
            Assert.That(() => circuit.RemoveComponent(true_constant), Throws.Nothing);
            Assert.That(() => circuit.RemoveComponent(true_constant), Throws.TypeOf<KeyNotFoundException>());
            // Try re-adding again:
            Assert.That(() => circuit.AddComponent(true_constant), Throws.Nothing);
        }

        [Test]
        public void Test_CircuitSimulate_SelfCycle()
        {
            LogicComponent not_gate = new NotGate();
            LogicComponent connection = new Connection();
            Circuit circuit = new Circuit();

            /* The circuit is just a not gate whose output is connected to its own input by a wire */
            circuit.AddComponent(not_gate);
            circuit.AddComponent(connection);
            circuit.Connect(not_gate, 0, connection, 0);
            circuit.Connect(connection, 0, not_gate, 0);

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
                circuit.Simulate();
                Assert.AreEqual(not_gate.Outputs, new List<bool>() { true });
                // After second step, connection should have true output too:
                circuit.Simulate();
                Assert.AreEqual(connection.Outputs, new List<bool>() { true });
                // After third step, not gate should have false output again:
                circuit.Simulate();
                Assert.AreEqual(not_gate.Outputs, new List<bool>() { false });
                // After fourth step, connection should have false output again:
                circuit.Simulate();
                Assert.AreEqual(connection.Outputs, new List<bool>() { false });
            }
        }

        [Test]
        public void Test_CircuitSimulate_SmallTree()
        {
            LogicComponent true_constant1 = new TrueConst();
            LogicComponent true_constant2 = new TrueConst();
            LogicComponent false_constant = new FalseConst();
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
            circuit.AddComponent(true_constant1);
            circuit.AddComponent(true_constant2);
            circuit.AddComponent(false_constant);
            circuit.AddComponent(and_gate);
            circuit.AddComponent(or_gate);
            circuit.AddComponent(true_constant1_connection);
            circuit.AddComponent(true_constant2_connection);
            circuit.AddComponent(false_constant_connection);
            circuit.AddComponent(and_gate_connection);

            circuit.Connect(true_constant1, 0, true_constant1_connection, 0);
            circuit.Connect(true_constant2, 0, true_constant2_connection, 0);
            circuit.Connect(true_constant1_connection, 0, and_gate, 0);
            circuit.Connect(true_constant2_connection, 0, and_gate, 1);
            circuit.Connect(and_gate, 0, and_gate_connection, 0);
            circuit.Connect(false_constant, 0, false_constant_connection, 0);
            circuit.Connect(and_gate_connection, 0, or_gate, 0);
            circuit.Connect(false_constant_connection, 0, or_gate, 1);

            // Try first step: the output of the connections from the constants should change.
            circuit.Simulate();
            Assert.AreEqual(true_constant1_connection.Outputs, new List<bool>() { true });
            Assert.AreEqual(true_constant2_connection.Outputs, new List<bool>() { true });
            // Try second step: the output from the and gate should change.
            circuit.Simulate();
            Assert.AreEqual(and_gate.Outputs, new List<bool>() { true });
            // Try third step: the output of the connections from the and gate should change.
            circuit.Simulate();
            Assert.AreEqual(and_gate_connection.Outputs, new List<bool>() { true });
            // Try fourth step: the output of the or gate should change.
            circuit.Simulate();
            Assert.AreEqual(or_gate.Outputs, new List<bool>() { true });

            // Ensure the state has stabilized into the expected state:
            for (int i = 0; i < 100; i++)
            {
                circuit.Simulate();
                Assert.AreEqual(true_constant1_connection.Outputs, new List<bool>() { true });
                Assert.AreEqual(true_constant2_connection.Outputs, new List<bool>() { true });
                Assert.AreEqual(and_gate.Outputs, new List<bool>() { true });
                Assert.AreEqual(and_gate_connection.Outputs, new List<bool>() { true });
                Assert.AreEqual(or_gate.Outputs, new List<bool>() { true });
            }
        }

        [Test]
        public void Test_RemoveComponent()
        {
            Circuit circuit = new Circuit();
            var and_gate = new AndGate();
            var true_const = new TrueConst();

            circuit.AddComponent(and_gate);
            circuit.AddComponent(true_const);
            circuit.Connect(true_const, 0, and_gate, 0);
            circuit.Connect(and_gate, 0, and_gate, 1);

            circuit.RemoveComponent(and_gate);
            circuit.RemoveComponent(true_const);
        }
    }
}
