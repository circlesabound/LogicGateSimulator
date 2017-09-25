using System.Collections;
using System.Collections.Generic;

public class FalseConstant : LogicComponent{
    public FalseConstant() : base(0, 1) { }

    public override List<bool> Simulate() => new List<bool> { false };
}
