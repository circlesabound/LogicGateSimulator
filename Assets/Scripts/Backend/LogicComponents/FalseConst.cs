using System.Collections;
using System.Collections.Generic;

public class FalseConst : LogicComponent{
    public FalseConst() : base(0, 1) {
        this.Outputs[0] = false;
    }

    protected override List<bool> CoreSimulate(IList<bool> inputs)
    {
        return new List<bool> { false };
    }
}
