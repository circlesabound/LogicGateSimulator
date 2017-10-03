using System;
using System.Collections.Generic;
using System.Linq;

public class CircuitGraph
{
    private Dictionary<LogicComponent, CircuitNode> Nodes;

    public CircuitGraph()
    {
        this.Nodes = new Dictionary<LogicComponent, CircuitNode>();
    }

    public void AddNode(LogicComponent component)
    {
        if (Nodes.ContainsKey(component))
        {
            throw new ArgumentException("Component already exists in circuit");
        }
        Nodes[component] = new CircuitNode(component);
    }

    public ICollection<LogicComponent> Components
    {
        get { return Nodes.Keys; }
    }

    public void AddEdge(LogicComponent out_component, int out_id,
            LogicComponent in_component, int in_id)
    {
        var out_node = Nodes[out_component];
        var in_node = Nodes[in_component];
        var edge = new CircuitEdge(out_node, out_id, in_node, in_id);
        out_node.AddEdgeOut(edge);
        in_node.AddEdgeIn(edge);
    }

    public IEnumerable<ConnectionEndpoint> GetInputs(LogicComponent component)
    {
        var node = Nodes[component];
        return node.AdjListIn.Select(
            edge => new ConnectionEndpoint(edge)
        );
    }

    public void RemoveEdge(LogicComponent out_component, int out_id,
            LogicComponent in_component, int in_id)
    {
        var out_node = Nodes[out_component];
        var in_node = Nodes[in_component];
        this.RemoveCircuitEdge(new CircuitEdge(out_node, out_id, in_node, in_id));
    }

    private void RemoveCircuitEdge(CircuitEdge edge)
    {
        if (edge == null) return;
        if (!edge.OutNode.HasEdgeOut(edge) || !edge.InNode.HasEdgeIn(edge))
        {
            throw new ArgumentException("Given edge does not exist");
        }
        edge.OutNode.RemoveEdgeOut(edge.OutId);
        edge.InNode.RemoveEdgeIn(edge.InId);
    }

    public void RemoveNode(LogicComponent component)
    {
        var node = Nodes[component];
        for (int i = 0; i < node.AdjListIn.Count; i++) {
            this.RemoveCircuitEdge(node.AdjListIn[i]);
        }
        for (int i = 0; i < node.AdjListOut.Count; i++) {
            this.RemoveCircuitEdge(node.AdjListOut[i]);
        }
        Nodes.Remove(component);
    }
}