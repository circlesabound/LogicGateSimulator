using System.Collections;
using System.Collections.Generic;

public class Output : LogicComponent {
    public bool Value { get; private set; }

    public Output() : base(1, 0) {
    }

    public override void Reset()
    {
        this.Value = false;
    }

    protected override List<bool> CoreSimulate(IList<bool> inputs)
    {
        this.Value = inputs[0];
        return new List<bool> {};
    }
}
