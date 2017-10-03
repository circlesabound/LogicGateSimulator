using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

namespace Assets.Scripts.ScratchPad
{
    public abstract class SPBinaryLogicComponent : SPLogicComponent
    {
        protected SPConnector InConnectorTop
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

        protected SPConnector InConnectorBottom
        {
            get
            {
                return InConnectors[1];
            }
            set
            {
                InConnectors[1] = value;
            }
        }

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

        protected override void Awake()
        {
            base.Awake();
            InConnectors.AddRange(Enumerable.Repeat<SPConnector>(null, 2));
            OutConnectors.AddRange(Enumerable.Repeat<SPConnector>(null, 1));

            // Set up connectors
            InConnectorTop = Instantiate(SPInConnectorPrefab, gameObject.transform, false);
            Assert.IsNotNull(InConnectorTop);
            InConnectorTop.gameObject.name = "InConnectorTop";
            InConnectorTop.transform.localPosition = new Vector3(-1, 1, -1);
            InConnectorTop.Register(this, SPConnectorType.SPInConnector, 0);

            InConnectorBottom = Instantiate(SPInConnectorPrefab, gameObject.transform, false);
            Assert.IsNotNull(InConnectorBottom);
            InConnectorBottom.gameObject.name = "InConnectorBottom";
            InConnectorBottom.transform.localPosition = new Vector3(-1, -1, -1);
            InConnectorBottom.Register(this, SPConnectorType.SPInConnector, 1);

            OutConnector = Instantiate(SPOutConnectorPrefab, gameObject.transform, false);
            Assert.IsNotNull(OutConnector);
            OutConnector.gameObject.name = "OutConnector";
            OutConnector.transform.localPosition = new Vector3(1, 0, -1);
            OutConnector.Register(this, SPConnectorType.SPOutConnector, 0);
        }
    }
}
