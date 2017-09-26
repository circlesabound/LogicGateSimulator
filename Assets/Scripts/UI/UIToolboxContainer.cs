using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.Assertions;

namespace Assets.Scripts.UI
{
    public class UIToolboxContainer : MonoBehaviour
    {
        public UIToolboxPanel UIToolboxPanelPrefab;
        private const string TOOLBOX_CONFIG_RESOURCE = "Configs/toolbox";
        private UIToolboxPanel _CurrentPanel;

        public UIToolboxPanel CurrentPanel
        {
            get
            {
                return this._CurrentPanel;
            }
            set
            {
                // Sanity check
                Assert.IsTrue(this.Panels.Contains(value));

                // Hide all panels
                foreach (var p in this.Panels) p.Hide();

                // Unhide new current panel
                value.Show();

                // State update
                this._CurrentPanel = value;
            }
        }

        public UIToolboxPanel MainPanel
        {
            get
            {
                return this.Panels.Find(p => p.IsMainPanel);
            }
        }

        public List<UIToolboxPanel> Panels
        {
            get;
            private set;
        }

        public ReadOnlyCollection<UIToolboxPanel> SubPanels
        {
            get
            {
                return this.Panels.FindAll(p => !p.IsMainPanel).AsReadOnly();
            }
        }

        private void Awake()
        {
            // Sanity checks
            Assert.IsNotNull(this.UIToolboxPanelPrefab);

            // Initialise internal state
            this.name = "UIToolboxContainer";
            this.tag = "UIToolboxContainer";
            this.Panels = new List<UIToolboxPanel>();

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
                this.Panels.Add(panel);
            }

            // Instantiate and build main panel
            Assert.IsNull(this.MainPanel);
            UIToolboxPanel mainPanel = Instantiate(this.UIToolboxPanelPrefab, this.transform);
            Assert.IsNotNull(mainPanel);
            mainPanel.Build(toolboxConfig);
            this.Panels.Add(mainPanel);

            // Finalise toolbox
            this.CurrentPanel = this.MainPanel;
        }

        // Use this for initialization
        private void Start()
        {
            //
        }

        // Update is called once per frame
        private void Update()
        {
            //
        }
    }
}
