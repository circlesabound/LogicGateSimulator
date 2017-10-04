using System.Collections.Generic;
using System.Linq;

public class CircuitNode
{
    public LogicComponent Component { get; set; }
    public List<CircuitEdge> AdjListOut { get; private set; }
    public List<CircuitEdge> AdjListIn { get; private set; }

    public CircuitNode(LogicComponent component)
    {
        this.Component = component;
        this.AdjListIn = Enumerable.Repeat<CircuitEdge>(null, component.NumInputs)
            .ToList();
        this.AdjListOut = Enumerable.Repeat<CircuitEdge>(null, component.NumOutputs)
            .ToList();
    }

    public void AddEdgeOut(CircuitEdge edge)
    {
        this.AdjListOut[edge.OutId] = edge;
    }
    public bool HasEdgeOut(CircuitEdge edge)
    {
        return this.AdjListOut[edge.OutId] == edge;
    }
    public void RemoveEdgeOut(int out_id)
    {
        this.AdjListOut[out_id] = null;
    }
    public void AddEdgeIn(CircuitEdge edge)
    {
        this.AdjListIn[edge.InId] = edge;
    }
    public bool HasEdgeIn(CircuitEdge edge)
    {
        return this.AdjListIn[edge.InId] == edge;
    }
    public void RemoveEdgeIn(int in_id)
    {
        this.AdjListIn[in_id] = null;
    }
}