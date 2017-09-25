using System.Collections;
using System.Collections.Generic;

public class Connection : LogicComponent
{
    public Connection() : base(1, 1) { }

    public override List<bool> Simulate()
    {
        return new List<bool> { this.Inputs[0].Value };
    }
}
