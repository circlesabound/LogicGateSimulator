using System.Collections;
using System.Collections.Generic;

public class TrueConstant : LogicComponent {
    public TrueConstant() : base(0, 1) {
        this.Outputs[0] = true;
    }

    protected override List<bool> CoreSimulate(IList<bool> inputs)
    {
        return new List<bool> { true };
    }
}
