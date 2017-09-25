using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;

public class NewEditModeTest {
    
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
    public void Test_ConstantComponents()
    {
        LogicComponent true_constant = new TrueConstant();
        Assert.AreEqual(true_constant.Outputs, new List<bool>() { true });
        Assert.AreEqual(true_constant.Simulate(), new List<bool>{ true });

        LogicComponent false_constant = new FalseConstant();
        Assert.AreEqual(false_constant.Outputs, new List<bool>() { false });
        Assert.AreEqual(false_constant.Simulate(), new List<bool> { false });
    }

    /// <summary>
    /// Tests whether a component has a specified truth table.
    /// </summary>
    /// <param name="component">The component to test</param>
    /// <param name="truth_table">
    /// A 2*2 truth table, where truth_table[f][s] is the expected value
    /// when the first input is f and the second input is s.</param>
    private void Test_TruthTable(LogicComponent component, bool[,] truth_table)
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
                Assert.AreEqual(component.Simulate(), new List<bool>() { truth_table[f,s] },
                                "Failed to simulate component with inputs: " + f + " and " + s);
            }
        }
    }
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

        Test_TruthTable(and_gate, new bool[2,2]{ { false, false}, { false, true} });
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

        Test_TruthTable(or_gate, new bool[2, 2] { { false, true }, { true, true } });
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

        Test_TruthTable(xor_gate, new bool[2, 2] { { false, true }, { true, false } });
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
    public void Test_InputComponent()
    {
        InputComponent input_component = new InputComponent();

        input_component.SetValue(true);
        Assert.AreEqual(input_component.Simulate(), new List<bool> { true });
        input_component.SetValue(false);
        Assert.AreEqual(input_component.Simulate(), new List<bool> { false });
    }
    public void Test_CircuitSimulate()
    {

    }
	// A UnityTest behaves like a coroutine in PlayMode
	// and allows you to yield null to skip a frame in EditMode
	[UnityTest]
	public IEnumerator NewEditModeTestWithEnumeratorPasses() {
		// Use the Assert class to test conditions.
		// yield to skip a frame
		yield return null;
	}
}
