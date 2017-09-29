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
        protected override void Start()
        {
            base.Start();
            LogicComponent = new XorGate();
            Canvas.Circuit.AddComponent(LogicComponent);
        }

        // Update is called once per frame
        protected override void Update()
        {
        }
    }
}
