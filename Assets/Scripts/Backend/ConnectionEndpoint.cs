using System.Collections;
using System.Collections.Generic;

public class ConnectionEndpoint {
    public LogicComponent Component { get; set; }
    public int OutputId { get; set; }
    public bool Value {
        get
        {
            if (Component == null)
            {
                return false;
            }
            else
            {
                return this.Component.Outputs[this.OutputId];
            }
        }
    }

    public ConnectionEndpoint()
    {
        Component = null;
    }
    
    public ConnectionEndpoint(LogicComponent component, int outputId)
    {
        this.Component = component;
        this.OutputId = outputId;
    }
}
