using System;
using System.Collections;
using System.Collections.Generic;

public class Clock : LogicComponent
{
    private uint _period;

    public uint Tick { get; private set; }
    public uint Period
    {
        get { return this._period; }
        set
        {
            if (value == 0)
            {
                throw new ArgumentOutOfRangeException("Period cannot be zero");
            }
            this._period = value;
        }
    }

    public Clock(uint period) : base(0, 1)
    {
        this.Outputs[0] = false;
        this.Tick = 0;
        this.Period = period;
    }

    public override void Reset()
    {
        this.Outputs[0] = false;
        this.Tick = 0;
    }

    protected override List<bool> CoreSimulate(IList<bool> inputs)
    {
        this.Tick++;
        if (this.Tick >= Period)
        {
            this.Tick = 0;
            return new List<bool> { !this.Outputs[0] };
        }
        return new List<bool> { this.Outputs[0] };
    }
}
