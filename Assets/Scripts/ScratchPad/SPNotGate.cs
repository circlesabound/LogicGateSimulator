using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Assertions;

namespace Assets.Scripts.ScratchPad
{
    public class SPNotGate : SPLogicComponent
    {
        protected SPConnector InConnector;
        protected SPConnector OutConnector;

        protected SPNotGate() : base()
        {
        }

        protected override void Awake()
        {
            // Set up connectors
            InConnector = Instantiate(SPInConnectorPrefab, gameObject.transform, false);
            Assert.IsNotNull(InConnector);
            InConnector.gameObject.name = "InConnector";
            InConnector.transform.localPosition = new Vector3(-1, 0, -1);
            InConnector.Register(this, SPConnectorType.SPInConnector, 0);

            OutConnector = Instantiate(SPOutConnectorPrefab, gameObject.transform, false);
            Assert.IsNotNull(OutConnector);
            OutConnector.gameObject.name = "OutConnector";
            OutConnector.transform.localPosition = new Vector3(1, 0, -1);
            OutConnector.Register(this, SPConnectorType.SPOutConnector, 0);
        }

        // Use this for initialisation
        protected override void Start()
        {
            base.Start();
            LogicComponent = new NotGate();
            Canvas.Circuit.AddComponent(LogicComponent);
        }

        // Update is called once per frame
        protected override void Update()
        {
        }
    }
}
