﻿using System;
using System.Linq;
using System.Collections.Generic;

public abstract class LogicComponent {
	protected List<ConnectionEndpoint> Inputs;
	public List<bool> Outputs { get; set;}

	public LogicComponent(int n_inputs, int n_outputs)
	{
        Inputs = Enumerable.Range(0, n_inputs)
            .Select(i => new ConnectionEndpoint())
            .ToList();
		//this.Inputs = new List<ConnectionEndpoint>(
		//	Enumerable.Repeat<ConnectionEndpoint>(null, n_inputs));
		this.Outputs = new List<bool>(Enumerable.Repeat(false, n_outputs));
	}

	public void AddInput(int input_id, LogicComponent component, int output_id)
	{
		this.Inputs[input_id] = new ConnectionEndpoint(component, output_id);
	}

	public void RemoveInput(int input_id)
	{
        this.Inputs[input_id] = new ConnectionEndpoint();
	}

	public abstract List<bool> Simulate();
}