using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;

namespace Assets.Scripts.ScratchPad
{
    public class SPNumberedInputToggler : SPLogicComponent, IPointerClickHandler
    {
        // TODO: DELETE THESE AFTER LINKING THE BELOW.
        public Sprite TrueSprite;
        public Sprite FalseSprite;

        public Sprite SelectedTrueSprite;
        public Sprite SelectedFalseSprite;

        // sprites linked in unity inspector
        public List<Sprite> UnselectedTrueSprites;
        public List<Sprite> SelectedTrueSprites;

        public List<Sprite> UnselectedFalseSprites;
        public List<Sprite> SelectedFalseSprites;

        private bool Selected;
        // The id of this component.
        // id == 0 means that it's not a numbered component
        public uint id;

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

        protected SPNumberedInputToggler() : base()
        {
        }

        public override void Delete()
        {
            //Debug.Log("SPNumberedInputToggler deleted");
            // Delete any incoming/outgoing edges
            var edgeList = Enumerable
                .Concat(InConnectors, OutConnectors)
                .Select(c => c.ConnectedEdge)
                .Where(e => e != null)
                .ToList();
            for (int i = 0; i < edgeList.Count; ++i)
            {
                edgeList[i].Delete();
            }

            // Delete myself
            Canvas.Components.Remove(this);

            // If id is still 0 then I'm not a numbered component.
            if (this.id != 0)
            {
                Canvas.RemoveNumberedComponent(this);
            }
            else
            {
                Canvas.Circuit.RemoveComponent(LogicComponent);
            }

            Destroy(this.gameObject);
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
            Debug.Log("In numbered input awake");
            OutConnectors.AddRange(Enumerable.Repeat<SPConnector>(null, 1));

            // Set up connectors
            OutConnector = Instantiate(SPOutConnectorPrefab, gameObject.transform, false);
            Assert.IsNotNull(OutConnector);
            OutConnector.gameObject.name = "OutConnector";
            OutConnector.transform.localPosition = new Vector3(1, 0, -1);
            OutConnector.Register(this, SPConnectorType.SPOutConnector, 0);

            LogicComponent = new InputComponent();
            try
            {
                this.id = Canvas.AddNumberedComponent(this);
            }
            catch (NoMoreIdsException)
            {
                // Failed to add this component as a numbered component.
                // Just leave its id as 0.
                this.id = 0;
                Canvas.Circuit.AddComponent(LogicComponent);
            }
        }

        public override int GetHashCode()
        {
            // TODO: Make hash take id into account?
            return base.GetHashCode() ^ ((InputComponent)LogicComponent).value.GetHashCode();
        }

        public void ToggleValue()
        {
            InputComponent inputComponent = (InputComponent)LogicComponent;
            inputComponent.FlipValue();
        }

        protected override void Update()
        {
            // TODO: ADD MULTIPLE SPRITES BASED ON id.
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
            // TODO: Delete this:
            Debug.Log("id is: " + id.ToString());
            Selected = true;
            // TODO: Configure output based on id?
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
