using System.Collections;
using System.Collections.Generic;

public class OrGate : LogicComponent
{
    public OrGate() : base(2, 1) { }

    public override List<bool> Simulate(IList<bool> inputs)
    {
        return new List<bool> { inputs[0] || inputs[1] };
    }
}
