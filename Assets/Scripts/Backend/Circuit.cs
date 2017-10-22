using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Circuit {
    private CircuitGraph graph;
    private Dictionary<uint, InputComponent> NumberedInputs;
    private Dictionary<uint, Output> NumberedOutputs;

    public Circuit()
    {
        graph = new CircuitGraph();
        NumberedInputs = new Dictionary<uint, InputComponent>();
        NumberedOutputs = new Dictionary<uint, Output>();
    }
    /// <summary>
    /// Adds a component to the circuit.
    /// The same component can't be added more than once.
    /// </summary>
    /// <param name="component">The component to add.</param>
    /// <exception cref="System.ArgumentException">Thrown if the component has already been added</exception>
    public void AddComponent(LogicComponent component)
    {
        this.graph.AddNode(component);
    }

    /// <summary>
    /// Adds a numbered component to the circuit.
    /// The same component can't be added more than once.
    /// Also 2 components with the same type and id can not exist concurrently.
    /// </summary>
    /// <param name="component">The component to add.</param>
    /// <param name="id">The id of the component to add.</param>
    /// <exception cref="System.ArgumentException">Thrown if the component has already been added
    /// or a component with this id already exists
    /// or the component is not of a type that can be numbered</exception>
    public void AddNumberedComponent(LogicComponent component, uint id)
    {
        var inputComponent = component as InputComponent;
        var outputComponent = component as Output;
        if (inputComponent != null)
        {
            if (NumberedInputs.ContainsKey(id))
            {
                throw new ArgumentException("id already in use");
            }
            this.graph.AddNode(component);
            NumberedInputs.Add(id, inputComponent);
        }
        else if (outputComponent != null)
        {
            if (NumberedOutputs.ContainsKey(id))
            {
                throw new ArgumentException("id already in use");
            }
            this.graph.AddNode(component);
            NumberedOutputs.Add(id, outputComponent);
        }
        else
        {
            throw new ArgumentException("component is of a type that can't be numbered");
        }
    }

/*
    /// <summary>
    /// Changes the assigned id of a component.
    /// Works for both input and output components.
    /// </summary>
    /// <param name="component">The component to change the id of.</param>
    /// <param name="newid">The new id of the component.</param>
    /// <exception cref="System.ArgumentException">Thrown if the component does not already have an id. </exception>
    public void RenumberComponent(LogicComponent component, uint newid)
    {
        bool found = false;
        var inputComponent = component as InputComponent;
        var outputComponent = component as OutputComponent;
        if (inputComponent != null)
        {
            try {
                // Throws if the component doesn't exist in the dictionary.
                var item = NumberedInputs.First(kvp => kvp.Value == component);
                NumberedInputs.Remove(item.Key);
                NumberedInputs.Add(newid, item.Value);
                found = true;
            }
            catch
            {
            }
        }
        if (outputComponent != null)
        {
            Assert.IsFalse(found);
            try {
                // Throws if the component doesn't exist in the dictionary.
                var item = NumberedOutputs.First(kvp => kvp.Value == component);
                NumberedOutputs.Remove(item.Key);
                NumberedOutputs.Add(newid, item.Value);
                found = true;
            }
            catch
            {
            }
        }
        if (!found)
        {
            throw new ArgumentException("component doesn't already have an id");
        }
    }
*/


    /// <summary>
    /// Removes a component.
    /// Will also free its id automatically if it is a numbered component.
    /// </summary>
    /// <param name="component">The component to remove.</param>
    /// <exception cref="System.Collections.Generic.KeyNotFoundException">Thrown if component does not exist</exception>
    public void RemoveComponent(LogicComponent component)
    {
        this.graph.RemoveNode(component);
        foreach (var item in NumberedInputs.Where(kvp => kvp.Value == component).ToList())
        {
            NumberedInputs.Remove(item.Key);
        }
        foreach (var item in NumberedOutputs.Where(kvp => kvp.Value == component).ToList())
        {
            NumberedOutputs.Remove(item.Key);
        }
    }

    /// <summary>
    /// Connects an output of a component to the input of another component
    /// </summary>
    /// <param name="out_component">The output component</param>
    /// <param name="out_id">The id of the output to connect from</param>
    /// <param name="in_component">The input component</param>
    /// <param name="in_id">The id of the input to connect to</param>
    /// <exception cref="System.Collections.Generic.KeyNotFoundException">Thrown if either component does not exist</exception>
    /// <exception cref="System.ArgumentOutOfRangeException">Thrown if either ids are out of range for the component</exception>
    public void Connect(LogicComponent out_component, int out_id,
        LogicComponent in_component, int in_id)
    {
        this.graph.AddEdge(out_component, out_id, in_component, in_id);
    }

    /// <summary>
    /// Disconnects an output of a component to the input of another component
    /// </summary>
    /// <param name="out_component">The output component</param>
    /// <param name="out_id">The id of the output to connect from</param>
    /// <param name="in_component">The input component</param>
    /// <param name="in_id">The id of the input to connect to</param>
    /// <exception cref="System.Collections.Generic.KeyNotFoundException">Thrown if either component does not exist</exception>
    /// <exception cref="System.ArgumentOutOfRangeException">Thrown if either ids are out of range for the component</exception>
    public void Disconnect(LogicComponent out_component, int out_id,
        LogicComponent in_component, int in_id)
    {
        this.graph.RemoveEdge(out_component, out_id, in_component, in_id);
    }

    /// <summary>
    /// Resets the components of the circuit to some base state
    /// (not necessarily the default state).
    /// </summary>
    public void ResetComponents()
    {
        var components = this.graph.Components;
        foreach (LogicComponent component in components)
        {
            component.Reset();
        }
    }

    /// <summary>
    /// Simulates one step of the circuit.
    /// </summary>
    public void Simulate()
    {
        var result_map = new Dictionary<LogicComponent, List<bool>>();
        var components = this.graph.Components;
        foreach (LogicComponent component in components)
        {
            var inputs = this.graph.GetInputs(component).Select(
                output => output.Value
            ).ToList();
            List<bool> simulation_results = component.Simulate(inputs);
            result_map[component] = simulation_results;
        }
        foreach (LogicComponent component in components)
        {
            component.Outputs = result_map[component];
        }
    }

    public bool Validate()
    {
        var stack = new Stack<LogicComponent>();
        var contributers = new HashSet<LogicComponent>();
        foreach (LogicComponent component in this.graph.Components)
        {
            Output output = component as Output;
            if (output == null) continue;
            if (!output.Value) return false;
            stack.Push(component);
        }
        while (stack.Count > 0)
        {
            var component = stack.Pop();
            contributers.Add(component);
            var inputs = this.graph.GetInputs(component).Select(
                output => output.Component);
            foreach (var input in inputs)
            {
                if (input == null) continue;
                if (contributers.Contains(input)) continue;
                stack.Push(input);
            }
        }
        return contributers.Count == this.graph.Components.Count;
    }
}
