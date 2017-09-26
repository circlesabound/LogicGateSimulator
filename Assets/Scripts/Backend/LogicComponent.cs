using System;
using System.Linq;
using System.Collections.Generic;

public abstract class LogicComponent
{
    public List<bool> Outputs { get; set; }
    public int NumInputs { get; private set; }
    public int NumOutputs
    {
        get { return Outputs.Count; }
    }

    public LogicComponent(int n_inputs, int n_outputs)
    {
        this.NumInputs = n_inputs;
        this.Outputs = Enumerable.Repeat(false, n_outputs).ToList();
    }

    public abstract List<bool> Simulate(IList<bool> inputs);
}
