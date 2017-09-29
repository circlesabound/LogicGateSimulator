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

        // Use this for initialisation
        protected override void Start()
        {
            LogicComponent = new AndGate();
        }

        // Update is called once per frame
        protected override void Update()
        {
        }
    }
}
