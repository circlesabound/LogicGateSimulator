using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

namespace Assets.Scripts.ScratchPad
{
    public class SPOutput : SPLogicComponent
    {
        private Sprite TrueSprite;
        private Sprite FalseSprite;
        private SpriteRenderer spriteRenderer;

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

            LogicComponent = new Output();
            Canvas.Circuit.AddComponent(LogicComponent);

            TrueSprite = Resources.Load<Sprite>("Sprites/outTrue");
            FalseSprite = Resources.Load<Sprite>("Sprites/out");
            spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        }

        protected override void Update()
        {
            base.Update();
            Output outputComponent = (Output)LogicComponent;
            if (outputComponent.Value == true)
            {
                spriteRenderer.sprite = TrueSprite;
            }
            else
            {
                spriteRenderer.sprite = FalseSprite;
            }
        }
    }
}
