using System;

public class CircuitEdge : Tuple<CircuitNode, int, CircuitNode, int>
{
    public CircuitNode OutNode { get { return this.Item1; } }
    public int OutId { get { return this.Item2; } }
    public CircuitNode InNode { get { return this.Item3; } }
    public int InId { get { return this.Item4; } }

    public CircuitEdge(CircuitNode out_node, int out_id,
        CircuitNode in_node, int in_id) : base(out_node, out_id, in_node, in_id)
    {
    }
}