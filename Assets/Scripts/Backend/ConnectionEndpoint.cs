using System.Collections;
using System.Collections.Generic;

public class ConnectionEndpoint {
    public LogicComponent Component { get; set; }
	public int OutputId { get; set; }
	public bool Value {
		get => this.Component.Outputs[this.OutputId];
	}

	public ConnectionEndpoint()
	{
	}
	
	public ConnectionEndpoint(LogicComponent component, int outputId)
	{
		this.Component = component;
		this.OutputId = outputId;
	}
}
