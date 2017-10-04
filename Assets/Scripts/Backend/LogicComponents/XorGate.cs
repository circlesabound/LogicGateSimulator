using System.Collections;
using System.Collections.Generic;

public class XorGate : LogicComponent
{
    public XorGate() : base(2, 1) { }

    protected override List<bool> CoreSimulate(IList<bool> inputs)
    {
        return new List<bool> { inputs[0] ^ inputs[1] };
    }
}
