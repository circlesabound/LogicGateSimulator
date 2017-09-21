using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace Assets.Scripts.UI
{
    public class UIToolboxContainer : MonoBehaviour
    {
        private const string TOOLBOX_CONFIG_RESOURCE = "Configs/toolbox";

        public UIToolboxPanel UIToolboxPanelPrefab;

        private UIToolboxPanel _CurrentPanel;

        public UIToolboxPanel MainPanel
        {
            get;
            private set;
        }

        public IList<UIToolboxPanel> SubPanels
        {
            get;
            private set;
        }

        public UIToolboxPanel CurrentPanel
        {
            get
            {
                return this._CurrentPanel;
            }
            set
            {
                // Sanity check
                Assert.IsTrue(value == this.MainPanel || this.SubPanels.Contains(value));

                // Hide all panels
                this.MainPanel.Hide();
                foreach (var subpanel in this.SubPanels)
                {
                    subpanel.Hide();
                }

                // Unhide new current panel
                value.Show();

                // State update
                this._CurrentPanel = value;
            }
        }

        public IList<UIToolboxPanel> AllPanels
        {
            get
            {
                throw new NotImplementedException();//TODO
            }
        }

        private void Awake()
        {
            // Sanity checks
            Assert.IsNotNull(this.UIToolboxPanelPrefab);

            // Initialise internal state
            this.SubPanels = new List<UIToolboxPanel>();

            // Load toolbox configuration from JSON
            TextAsset toolboxConfigAsset = Resources.Load<TextAsset>(TOOLBOX_CONFIG_RESOURCE);
            Assert.IsNotNull(toolboxConfigAsset);
            UIToolboxConfig toolboxConfig = JsonUtility.FromJson<UIToolboxConfig>(toolboxConfigAsset.text);

            // Instantiate and build sub panels
            foreach (var subpanelConfig in toolboxConfig.panels)
            {
                UIToolboxPanel panel = Instantiate(this.UIToolboxPanelPrefab, this.transform);
                Assert.IsNotNull(panel);
                panel.Build(subpanelConfig);
                this.SubPanels.Add(panel);
            }

            // Instantiate and build main panel
            this.MainPanel = Instantiate(this.UIToolboxPanelPrefab, this.transform);
            Assert.IsNotNull(this.MainPanel);
            this.MainPanel.Build(toolboxConfig);

            // Finalise toolbox
            this.CurrentPanel = this.MainPanel;
        }

        // Use this for initialization
        private void Start()
        {
            this.gameObject.name = "UIToolboxContainer";
        }

        // Update is called once per frame
        private void Update()
        {
            //
        }
    }
}
