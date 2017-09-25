using System.Collections;
using System.Collections.Generic;

public class NotGate : LogicComponent
{
    public NotGate() : base(1, 1) { }

    public override List<bool> Simulate()
    {
        return new List<bool> { !this.Inputs[0].Value };
    }
}
