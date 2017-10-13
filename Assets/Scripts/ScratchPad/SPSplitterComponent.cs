using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

namespace Assets.Scripts.ScratchPad
{
    public class SPSplitterComponent : SPLogicComponent
    {
        protected SPConnector InConnector
        {
            get
            {
                return InConnectors[0];
            }
            set
            {
                InConnectors[0] = value;
            }
        }

        protected SPConnector OutConnectorTop
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

        protected SPConnector OutConnectorBottom
        {
            get
            {
                return OutConnectors[1];
            }
            set
            {
                OutConnectors[1] = value;
            }
        }

        protected override void Awake()
        {
            base.Awake();
            InConnectors.AddRange(Enumerable.Repeat<SPConnector>(null, 1));
            OutConnectors.AddRange(Enumerable.Repeat<SPConnector>(null, 2));

            // Set up connectors
            InConnector = Instantiate(SPInConnectorPrefab, gameObject.transform, false);
            Assert.IsNotNull(InConnector);
            InConnector.gameObject.name = "InConnector";
            InConnector.transform.localPosition = new Vector3(-1, 0, -1);
            InConnector.Register(this, SPConnectorType.SPInConnector, 0);

            OutConnectorTop = Instantiate(SPOutConnectorPrefab, gameObject.transform, false);
            Assert.IsNotNull(OutConnectorTop);
            OutConnectorTop.gameObject.name = "OutConnectorTop";
            OutConnectorTop.transform.localPosition = new Vector3(1, 1, -1);
            OutConnectorTop.Register(this, SPConnectorType.SPOutConnector, 0);

            OutConnectorBottom = Instantiate(SPOutConnectorPrefab, gameObject.transform, false);
            Assert.IsNotNull(OutConnectorBottom);
            OutConnectorBottom.gameObject.name = "OutConnectorBottom";
            OutConnectorBottom.transform.localPosition = new Vector3(1, -1, -1);
            OutConnectorBottom.Register(this, SPConnectorType.SPOutConnector, 1);
        }

        // Use this for initialisation
        protected override void Start()
        {
            base.Start();
            LogicComponent = new Splitter();
            Canvas.Circuit.AddComponent(LogicComponent);
        }
    }
}
