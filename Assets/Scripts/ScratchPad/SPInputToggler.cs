using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;

namespace Assets.Scripts.ScratchPad
{
    public class SPInputToggler : SPLogicComponent, IPointerClickHandler
    {

        private Sprite TrueSprite;
        private Sprite FalseSprite;

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
                Debug.Log("toggling value");
                InputComponent inputComponent = (InputComponent)LogicComponent;
                inputComponent.FlipValue();

                SpriteRenderer renderer = gameObject.GetComponent<SpriteRenderer>();
                if (inputComponent.value == true)
                {
                    renderer.sprite = TrueSprite;
                }
                else
                {
                    renderer.sprite = FalseSprite;
                }
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
        }

        // Use this for initialisation
        protected override void Start()
        {
            base.Start();
            LogicComponent = new InputComponent();
            Canvas.Circuit.AddComponent(LogicComponent);

            TrueSprite = Resources.Load<Sprite>("Sprites/true");
            FalseSprite = Resources.Load<Sprite>("Sprites/false");
        }


        // Update is called once per frame
        protected override void Update()
        {
        }
    }
}
