using System.Collections;
using System.Collections.Generic;

public class Splitter : LogicComponent
{
    public Splitter() : base(1, 2) { }

    protected override List<bool> CoreSimulate(IList<bool> inputs)
    {
        return new List<bool> { inputs[0], inputs[0] };
    }
}
