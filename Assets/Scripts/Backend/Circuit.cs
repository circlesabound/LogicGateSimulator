using System.Collections;
using System.Collections.Generic;

public class Circuit {
	private List<LogicComponent> components;

    /// <summary>
    /// Adds a component to the circuit.
    /// The same component can't be added more than once.
    /// </summary>
    /// <param name="component">The component to add.</param>
    /// <returns>True if the component was added successfully(was not in the circuit before). False otherwise</returns>
    public bool AddComponent(LogicComponent component)
    {
        if (components.Exists(item => item == component))
        {
            return false;
        } else
        {
            components.Add(component);
            return true;
        }
    }

    /// <summary>
    /// Removes a component.
    /// </summary>
    /// <param name="component">The component to remove.</param>
    /// <returns>True if the component was removed successfully, False otherwise.</returns>
    public bool RemoveComponent(LogicComponent component)
    {
        return components.Remove(component);
    }

    /// <summary>
    /// Simulates one step of the circuit.
    /// </summary>
    /// <returns>True if the simulate step succeeded, False otherwise.</returns>
    public bool Simulate()
    {
        var ResultMap = new Dictionary<LogicComponent, List<bool>>();
        foreach (LogicComponent component in components)
        {
            List<bool> simulation_results = component.Simulate();
            ResultMap[component] = simulation_results;
        }
        foreach (LogicComponent component in components)
        {
            component.Outputs = ResultMap[component];
        }
        return true;
    }
}
