using System.Collections;
using System.Collections.Generic;

public class Connection : LogicComponent
{
    public Connection() : base(1, 1) { }

    protected override List<bool> CoreSimulate(IList<bool> inputs)
    {
        return new List<bool> { inputs[0] };
    }
}
