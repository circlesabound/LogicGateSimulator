using UnityEngine.EventSystems;

namespace Assets.Scripts.ScratchPad
{
    public class SPOutConnector : SPConnector
    {
        protected override SPConnectorType ConnectorType
        {
            get
            {
                return SPConnectorType.SPOutConnector;
            }
        }

        public override void OnPointerClick(PointerEventData eventData)
        {
            // Unity doesn't like virtual event handlers
            // lame
            base.OnPointerClick(eventData);
        }
    }
}
