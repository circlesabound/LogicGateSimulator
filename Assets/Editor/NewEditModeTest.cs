using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

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
