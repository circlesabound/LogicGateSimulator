using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Circuit {
    private CircuitGraph graph;

    public Circuit()
    {
        graph = new CircuitGraph();
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
    /// Removes a component.
    /// </summary>
    /// <param name="component">The component to remove.</param>
    /// <exception cref="System.Collections.Generic.KeyNotFoundException">Thrown if component does not exist</exception>
    public void RemoveComponent(LogicComponent component)
    {
        this.graph.RemoveNode(component);
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
