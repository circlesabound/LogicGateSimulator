using System.Collections;
using System.Collections.Generic;

public class TrueConstant : LogicComponent {
    public TrueConstant() : base(0, 1) { }

    public override List<bool> Simulate() => new List<bool> { true };
}
