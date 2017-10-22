using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;

namespace Assets.Scripts.ScratchPad
{
    public class SPOutput : SPLogicComponent
    {
        public Sprite TrueSprite;
        public Sprite FalseSprite;

        public Sprite SelectedTrueSprite;
        public Sprite SelectedFalseSprite;

        private SpriteRenderer spriteRenderer;

        private bool Selected;

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

            spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        }

        protected override void Update()
        {
            base.Update();
            Output outputComponent = (Output)LogicComponent;
            if (outputComponent.Value == true)
            {
                if (Selected) spriteRenderer.sprite = SelectedTrueSprite;
                else spriteRenderer.sprite = TrueSprite;
            }
            else
            {
                if (Selected) spriteRenderer.sprite = SelectedFalseSprite;
                else spriteRenderer.sprite = FalseSprite;
            }
        }

        public override void OnPointerEnter(PointerEventData data)
        {
            Selected = true;
            InfoPanel.SetInfoTarget(this);
            InfoPanel.Show();
        }

        public override void OnPointerExit(PointerEventData data)
        {
            Selected = false;
            InfoPanel.Hide();
        }
    }
}
