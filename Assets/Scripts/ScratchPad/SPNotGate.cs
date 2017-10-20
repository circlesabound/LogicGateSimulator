using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;

namespace Assets.Scripts.ScratchPad
{
    public class SPNotGate : SPLogicComponent
    {
        protected SPNotGate() : base()
        {
        }

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

            InConnectors.AddRange(Enumerable.Repeat<SPConnector>(null, 1));
            OutConnectors.AddRange(Enumerable.Repeat<SPConnector>(null, 1));

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

            LogicComponent = new NotGate();
            Canvas.Circuit.AddComponent(LogicComponent);

        }

        public override void OnPointerEnter(PointerEventData data)
        {
            // TODO override with different sprite depending on clock tick
            base.OnPointerEnter(data);
        }

        public override void OnPointerExit(PointerEventData data)
        {
            // TODO override with different sprite depending on clock tick
            base.OnPointerExit(data);
        }
    }
}
