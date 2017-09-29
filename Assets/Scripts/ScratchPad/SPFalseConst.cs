using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Assertions;

namespace Assets.Scripts.ScratchPad
{
    public class SPFalseConst : SPLogicComponent
    {
        protected SPConnector OutConnector
        {
            get
            {
                return OutConnectors[0];
            }
            set
            {
                OutConnectors[0] = value;
            }
        }

        protected SPFalseConst() : base()
        {
        }

        protected override void Awake()
        {
            base.Awake();
            OutConnectors.AddRange(Enumerable.Repeat<SPConnector>(null, 1));

            // Set up connectors
            OutConnector = Instantiate(SPOutConnectorPrefab, gameObject.transform, false);
            Assert.IsNotNull(OutConnector);
            OutConnector.gameObject.name = "OutConnector";
            OutConnector.transform.localPosition = new Vector3(1, 0, -1);
        }

        // Use this for initialisation
        protected override void Start()
        {
            base.Start();
            LogicComponent = new FalseConst();
            Canvas.Circuit.AddComponent(LogicComponent);
        }

        // Update is called once per frame
        protected override void Update()
        {
        }
    }
}
