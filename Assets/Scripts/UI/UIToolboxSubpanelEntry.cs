using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;

namespace Assets.Scripts.UI
{
    /// <summary>
    /// An entry for a sub-panel in a toolbox panel
    /// </summary>
    public class UIToolboxSubpanelEntry : UIToolboxEntry
    {
        public const string NAME_PREFIX = "UIToolboxPanelEntry_";

        private UIToolboxPanel ReferencedPanel;

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

        public override void OnPointerClick(PointerEventData eventData)
        {
            base.OnPointerClick(eventData);
            GameObject.FindObjectOfType<UIToolboxContainer>().CurrentPanel = this.ReferencedPanel;
        }

        /// <summary>
        /// Assume referenced panel has been built at this stage
        /// </summary>
        protected new void Start()
        {
            base.Start();

            // There's probably a better way to do this
            UIToolboxContainer toolboxContainer = GameObject.FindObjectOfType<UIToolboxContainer>();
            Assert.IsNotNull(toolboxContainer);
            string referencedName = UIToolboxPanel.NAME_PREFIX + this.SimpleName;
            this.ReferencedPanel = toolboxContainer.Panels.Find(p => p.name == referencedName);
            Assert.IsNotNull(this.ReferencedPanel);
        }
    }
}
