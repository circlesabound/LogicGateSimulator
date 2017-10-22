using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;

namespace Assets.Scripts.ScratchPad
{
    public class SPNumberedOutput : SPLogicComponent
    {
        public Sprite TrueSprite;
        public Sprite FalseSprite;

        public Sprite SelectedTrueSprite;
        public Sprite SelectedFalseSprite;
        
        // The id of this component.
        // id == 0 means that it's not a numbered component
        public uint id;

        private bool Selected;

        protected SPNumberedOutput() : base()
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

        public override void Delete()
        {
            // Debug.Log("SPNumberedOutput deleted");
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

        protected override void Awake()
        {
            base.Awake();
            Debug.Log("In numbered output awake");
            InConnectors.AddRange(Enumerable.Repeat<SPConnector>(null, 1));

            // Set up connector
            InConnector = Instantiate(SPInConnectorPrefab, gameObject.transform, false);
            Assert.IsNotNull(InConnector);
            InConnector.gameObject.name = "InConnector";
            InConnector.transform.localPosition = new Vector3(-1, 0, -1);
            InConnector.Register(this, SPConnectorType.SPInConnector, 0);

            LogicComponent = new Output();
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
            return base.GetHashCode();
        }

        protected override void Update()
        {
            // TODO: ADD MULTIPLE SPRITES BASED ON id.
            base.Update();
            Output outputComponent = (Output)LogicComponent;
            SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
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
        }
    }
}
