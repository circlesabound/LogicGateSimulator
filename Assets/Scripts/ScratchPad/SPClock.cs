using Assets.Scripts.UI.MessageBoxes;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;

namespace Assets.Scripts.ScratchPad
{
    public class SPClock : SPLogicComponent, IMessageBoxTriggerTarget
    {
        private const string CLOCK_MESSAGE_BOX_CONFIG_RESOURCE = "Configs/MessageBoxes/clock";
        private const uint DEFAULT_CLOCK_RATE = 60;
        private MessageBoxConfig ClockMessageBoxConfig;
        private UIMessageBoxFactory MessageBoxFactory;

        protected SPClock() : base()
        {
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

        public override int GetHashCode()
        {
            return base.GetHashCode() ^ ((Clock)LogicComponent).Period.GetHashCode();
        }

        public override void OnPointerClick(PointerEventData eventData)
        {
            base.OnPointerClick(eventData);
            if (eventData.button == PointerEventData.InputButton.Left && !eventData.dragging)
            {
                // TODO: Link this to mutability of clock item.
                if (Canvas.CurrentTool == SPTool.Pointer &&
                    Canvas.CurrentMode != GameMode.ActivateAllOutputsChallenge)
                {
                    MessageBoxFactory.MakeFromConfig(ClockMessageBoxConfig, this);
                }
            }
        }

        public void Trigger(MessageBoxTriggerData triggerData)
        {
            if (triggerData.ButtonPressed == UIMessageBox.MessageBoxButtonType.Positive)
            {
                Assert.IsTrue(triggerData.NumberInput.HasValue);
                ((Clock)this.LogicComponent).Period = (uint)triggerData.NumberInput.Value;
                Debug.Log("Setting clock rate to " + ((Clock)this.LogicComponent).Period.ToString());
            }
            Destroy(triggerData.Sender.gameObject);
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

            MessageBoxFactory = new UIMessageBoxFactory();

            // Load the message box config for save ciruit
            TextAsset configAsset = Resources.Load<TextAsset>(CLOCK_MESSAGE_BOX_CONFIG_RESOURCE);
            Assert.IsNotNull(configAsset);
            ClockMessageBoxConfig = JsonUtility.FromJson<MessageBoxConfig>(configAsset.text);

            LogicComponent = new Clock(DEFAULT_CLOCK_RATE);
            Canvas.Circuit.AddComponent(LogicComponent);
        }

        // Update is called once per frame
        protected override void Update()
        {
            base.Update();
            //TODO swap sprites depending on clock state
        }
    }
}
