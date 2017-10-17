using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.ScratchPad
{
    public class SPOrGate : SPBinaryLogicComponent
    {
        protected SPOrGate() : base()
        {
        }

        // Use this for initialisation
        protected override void Awake()
        {
            base.Awake();
            LogicComponent = new OrGate();
            Canvas.Circuit.AddComponent(LogicComponent);
        }
    }
}
