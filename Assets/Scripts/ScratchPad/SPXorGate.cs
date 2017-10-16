using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.ScratchPad
{
    public class SPXorGate : SPBinaryLogicComponent
    {
        protected SPXorGate() : base()
        {
        }

        // Use this for initialisation
        protected override void Awake()
        {
            base.Awake();
            LogicComponent = new XorGate();
            Canvas.Circuit.AddComponent(LogicComponent);
        }
    }
}
