using UnityEngine.EventSystems;

namespace Assets.Scripts.UI
{
    /// <summary>
    /// An entry for a logic component in a toolbox panel
    /// </summary>
    public class UIToolboxComponent : UIToolboxEntry
    {
        public const string NAME_PREFIX = "UIToolboxComponent_";

        public string SimpleName
        {
            get
            {
                return this.name.Substring(NAME_PREFIX.Length);
            }
            set
            {
                this.name = NAME_PREFIX + value;
            }
        }

        public UILogicComponent LogicComponentPrefab;

        public override void OnPointerClick(PointerEventData eventData)
        {
            this.ToolboxPanel.ToggleSelectedEntry(this);
        }
    }
}
