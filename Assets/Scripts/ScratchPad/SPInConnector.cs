using UnityEngine.EventSystems;

namespace Assets.Scripts.ScratchPad
{
    public class SPInConnector : SPConnector
    {
        protected override SPConnectorType ConnectorType
        {
            get
            {
                return SPConnectorType.SPInConnector;
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
