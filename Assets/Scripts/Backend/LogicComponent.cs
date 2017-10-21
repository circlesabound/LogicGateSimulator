using System;
using System.Diagnostics;
using System.Linq;
using System.Collections.Generic;

public abstract class LogicComponent
{
    private List<bool> outputs;
    public List<bool> Outputs
    {
        get { return this.outputs; }
        set
        {
            if (value.Count != this.outputs.Count)
            {
                throw new ArgumentException("Unexpected number of outputs given");
            }
            this.outputs = value;
        }
    }
    public int NumInputs { get; private set; }
    public int NumOutputs
    {
        get { return Outputs.Count; }
    }

    public LogicComponent(int n_inputs, int n_outputs)
    {
        this.NumInputs = n_inputs;
        this.outputs = Enumerable.Repeat(false, n_outputs).ToList();
    }

    public virtual void Reset()
    {
        this.Outputs = Enumerable.Repeat(false, NumOutputs).ToList();
    }

    public List<bool> Simulate(IList<bool> inputs)
    {
        if (inputs.Count != this.NumInputs)
        {
            throw new ArgumentException("Unexpected number of inputs given");
        }
        var ret = this.CoreSimulate(inputs);
        Debug.Assert(ret.Count == NumOutputs);
        return ret;
    }

    protected abstract List<bool> CoreSimulate(IList<bool> inputs);
}
