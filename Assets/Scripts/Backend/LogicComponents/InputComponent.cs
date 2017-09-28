﻿using System.Collections;
using System.Collections.Generic;

public class InputComponent : LogicComponent
{
    private bool value;
    public InputComponent() : base(0, 1)
    {
        value = false;
        this.Outputs[0] = value;
    }
    public void SetValue(bool new_value)
    {
        value = new_value;
        this.Outputs[0] = value;
    }

    protected override List<bool> CoreSimulate(IList<bool> inputs)
    {
        return new List<bool> { value };
    }
}
