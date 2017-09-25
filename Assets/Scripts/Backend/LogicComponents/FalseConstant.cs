using System.Collections;
using System.Collections.Generic;

public class FalseConstant : LogicComponent{
    public FalseConstant() : base(0, 1) {
        this.Outputs[0] = false;
    }

    public override List<bool> Simulate()
    {
        return new List<bool> { false };
    }
}
