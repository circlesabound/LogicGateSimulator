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
        private UIToolboxPanel ReferencedPanel;

        private string _Name;

        public string Name
        {
            get
            {
                return this._Name;
            }
            set
            {
                this._Name = value;
            }
        }
        public string FullName
        {
            get
            {
                return "UIToolboxPanelEntry_" + this._Name;
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
            string referencedName = "UIToolboxPanel_" + this.Name;
            if (toolboxContainer.MainPanel.Name == referencedName)
            {
                this.ReferencedPanel = toolboxContainer.MainPanel;
            }
            else
            {
                foreach (var subpanel in toolboxContainer.SubPanels)
                {
                    if (subpanel.Name == referencedName)
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
