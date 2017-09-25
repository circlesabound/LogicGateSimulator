using System.Collections;
using System.Collections.Generic;

public class XorGate : LogicComponent
{
    public XorGate() : base(2, 1) { }

    public override List<bool> Simulate()
    {
        return new List<bool> { this.Inputs[0].Value ^ this.Inputs[1].Value };
    }
}
