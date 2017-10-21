using System.Collections;
using System.Collections.Generic;

public class TrueConst : LogicComponent {
    public TrueConst() : base(0, 1) {
        this.Outputs[0] = true;
    }

    public override void Reset()
    {
        this.Outputs[0] = true;
    }

    protected override List<bool> CoreSimulate(IList<bool> inputs)
    {
        return new List<bool> { true };
    }
}
