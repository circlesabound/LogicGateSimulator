using System;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;

namespace Assets.Scripts.UI
{
    /// <summary>
    /// An entry for a sub-panel in a toolbox panel
    /// </summary>
    public class UIToolboxPanelEntry : UIToolboxEntry
    {
        private const string NAME_PREFIX = "UIToolboxPanelEntry_";

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

        /// <summary>
        /// Assume referenced panel has been built at this stage
        /// </summary>
        protected new void Start()
        {
            base.Start();

            // There's probably a better way to do this
            UIToolboxContainer toolboxContainer = GameObject.FindObjectOfType<UIToolboxContainer>();
            Assert.IsNotNull(toolboxContainer);
            string referencedName = "UIToolboxPanel_" + this.SimpleName;
            if (toolboxContainer.MainPanel.name == referencedName)
            {
                this.ReferencedPanel = toolboxContainer.MainPanel;
            }
            else
            {
                foreach (var subpanel in toolboxContainer.SubPanels)
                {
                    if (subpanel.name == referencedName)
                    {
                        this.ReferencedPanel = subpanel;
                        break;
                    }
                }
            }
            Assert.IsNotNull(this.ReferencedPanel);
        }

        public override void OnPointerClick(PointerEventData eventData)
        {
            GameObject.FindObjectOfType<UIToolboxContainer>().CurrentPanel = this.ReferencedPanel;
        }
    }
}
