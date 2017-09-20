using UnityEngine.EventSystems;

namespace Assets.Scripts.UI
{
    /// <summary>
    /// An entry for a logic component in a toolbox panel
    /// </summary>
    public class UIToolboxComponent : UIToolboxEntry
    {
        public string Name
        {
            get
            {
                return this.Name;
            }
            set
            {
                this.name = "UIToolboxComponent_" + value;
            }
        }
        public UILogicComponent LogicComponentPrefab;

        public override void OnPointerClick(PointerEventData eventData)
        {
            this.ToolboxPanel.ToggleSelectedEntry(this);
        }
    }
}
