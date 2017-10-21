using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;

namespace Assets.Scripts.ScratchPad
{
    public class SPInputToggler : SPLogicComponent, IPointerClickHandler
    {
        public Sprite TrueSprite;
        public Sprite FalseSprite;

        public Sprite SelectedTrueSprite;
        public Sprite SelectedFalseSprite;

        private bool Selected;

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

        protected SPInputToggler() : base()
        {
        }

        public override void OnPointerClick(PointerEventData eventData)
        {
            Debug.Log(this.GetType().Name + "| " + Canvas.CurrentTool.ToString() + " | " + eventData.button.ToString() + " click");
            if (eventData.button == PointerEventData.InputButton.Left && !eventData.dragging)
            {
                ToggleValue();
            }

            base.OnPointerClick(eventData);
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
            OutConnector.Register(this, SPConnectorType.SPOutConnector, 0);

            LogicComponent = new InputComponent();
            Canvas.Circuit.AddComponent(LogicComponent);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode() ^ ((InputComponent)LogicComponent).value.GetHashCode();
        }

        public void ToggleValue()
        {
            InputComponent inputComponent = (InputComponent)LogicComponent;
            inputComponent.FlipValue();
        }

        protected override void Update()
        {
            base.Update();
            InputComponent inputComponent = (InputComponent)LogicComponent;
            SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
            if (inputComponent.value == true)
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
