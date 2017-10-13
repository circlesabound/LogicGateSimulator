using System;
using System.Collections;
using System.Collections.Generic;

public class Clock : LogicComponent{
    private uint tick;
    private uint period;

    public Clock(uint period) : base(0, 1) {
        this.Outputs[0] = false;
        this.tick = 0;
        if (period == 0)
        {
            throw new ArgumentOutOfRangeException("Period cannot be zero");
        }
        this.period = period;
    }

    protected override List<bool> CoreSimulate(IList<bool> inputs)
    {
        this.tick++;
        if (this.tick == period)
        {
            this.tick = 0;
            return new List<bool> { !this.Outputs[0] };
        }
        return new List<bool> { this.Outputs[0] };
    }
}
