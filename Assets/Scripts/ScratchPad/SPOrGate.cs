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
        protected override void Start()
        {
            base.Start();
            LogicComponent = new OrGate();
            Canvas.Circuit.AddComponent(LogicComponent);
        }

        // Update is called once per frame
        protected override void Update()
        {
        }
    }
}
