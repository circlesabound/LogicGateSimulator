using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Assertions;

namespace Assets.Scripts.ScratchPad
{
    public class SPAndGate : SPBinaryLogicComponent
    {
        protected SPAndGate() : base()
        {
        }

        protected override void Awake()
        {
            base.Awake();
            LogicComponent = new AndGate();
            Canvas.Circuit.AddComponent(LogicComponent);
        }
    }
}
