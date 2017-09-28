using UnityEngine;
using UnityEngine.Assertions;

namespace Assets.Scripts.ScratchPad
{
    public abstract class SPBinaryLogicComponent : SPLogicComponent
    {
        protected SPConnector InConnectorTop;
        protected SPConnector InConnectorBottom;
        protected SPConnector OutConnector;

        protected override void Awake()
        {
            // Set up connectors
            InConnectorTop = Instantiate(SPInConnectorPrefab, gameObject.transform, false);
            Assert.IsNotNull(InConnectorTop);
            InConnectorTop.gameObject.name = "InConnectorTop";
            InConnectorTop.transform.localPosition = new Vector3(-1, 1, -1);

            InConnectorBottom = Instantiate(SPInConnectorPrefab, gameObject.transform, false);
            Assert.IsNotNull(InConnectorBottom);
            InConnectorBottom.gameObject.name = "InConnectorBottom";
            InConnectorBottom.transform.localPosition = new Vector3(-1, -1, -1);

            OutConnector = Instantiate(SPOutConnectorPrefab, gameObject.transform, false);
            Assert.IsNotNull(OutConnector);
            OutConnector.gameObject.name = "OutConnector";
            OutConnector.transform.localPosition = new Vector3(1, 0, -1);
        }
    }
}
