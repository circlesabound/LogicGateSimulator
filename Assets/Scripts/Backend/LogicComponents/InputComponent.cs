using System.Collections;
using System.Collections.Generic;

public class InputComponent : LogicComponent
{
    public bool value
    {
        get;
        private set;
    }

    public InputComponent() : base(0, 1)
    {
        value = false;
        this.Outputs[0] = value;
    }

    public override void Reset()
    {
        this.Outputs[0] = value;
    }

    public void SetValue(bool new_value)
    {
        value = new_value;
        this.Outputs[0] = value;
    }
    public void FlipValue()
    {
        value = !value;
        this.Outputs[0] = value;
    }

    protected override List<bool> CoreSimulate(IList<bool> inputs)
    {
        return new List<bool> { value };
    }
}
