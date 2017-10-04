using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

namespace Assets.Scripts.ScratchPad
{
    public class SPOutput : SPLogicComponent
    {
        protected SPOutput() : base()
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

        protected override void Awake()
        {
            base.Awake();
            InConnectors.AddRange(Enumerable.Repeat<SPConnector>(null, 1));

            // Set up connector
            InConnector = Instantiate(SPInConnectorPrefab, gameObject.transform, false);
            Assert.IsNotNull(InConnector);
            InConnector.gameObject.name = "InConnector";
            InConnector.transform.localPosition = new Vector3(-1, 0, -1);
            InConnector.Register(this, SPConnectorType.SPInConnector, 0);
        }

        // Use this for initialisation
        protected override void Start()
        {
            base.Start();
            LogicComponent = new Output();
            Canvas.Circuit.AddComponent(LogicComponent);
        }

        // Update is called once per frame
        protected override void Update()
        {
        }
    }
}
