using System.Collections;
using System.Collections.Generic;

public class TrueConstant : LogicComponent {
    public TrueConstant() : base(0, 1) {
        this.Outputs[0] = true;
    }

    public override List<bool> Simulate()
    {
        return new List<bool> { true };
    }
}
