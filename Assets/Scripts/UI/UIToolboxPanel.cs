using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;

namespace Assets.Scripts.UI
{
    /// <summary>
    /// A panel in the toolbox
    /// </summary>
    public class UIToolboxPanel : MonoBehaviour
    {
        public UIToolboxPanelEntry UIToolboxPanelEntryPrefab;
        public UIToolboxComponent UIToolboxComponentPrefab;

        public string Name
        {
            get
            {
                return this.name;
            }
            set
            {
                this.name = "UIToolboxPanel_" + value;
            }
        }

        private List<UIToolboxEntry> Entries;
        private UIToolboxEntry CurrentlySelectedEntry;
        private bool Built;
        public bool IsMainPanel
        {
            get
            {
                return this.Name == "UIToolboxPanel_Main";
            }
        }
        
        private void Awake()
        {
            this.Entries = new List<UIToolboxEntry>();
            this.CurrentlySelectedEntry = null;
            this.Built = false;
        }

        private void Start()
        {
            if (this.IsMainPanel)
            {
                this.Show();
            }
            else
            {
                this.Hide();
            }
        }
        
        private void Update()
        {
            //
        }

        /// <summary>
        /// Build this panel as the main panel, populated with sub panel entries
        /// </summary>
        /// <param name="mainPanelConfig"></param>
        public void Build(UIToolboxConfig mainPanelConfig)
        {
            // Sanity check
            Assert.IsFalse(this.Built);
            Assert.IsNotNull(mainPanelConfig);
            Assert.IsNotNull(this.UIToolboxPanelEntryPrefab);

            // Set name
            this.Name = "Main";

            Debug.Log("Building " + this.Name + " as the main panel");
            foreach (var panelEntryConfig in mainPanelConfig.panels)
            {
                // Load sprites
                Sprite activeSprite = Resources.Load<Sprite>(panelEntryConfig.sprite_selected);
                Sprite inactiveSprite = Resources.Load<Sprite>(panelEntryConfig.sprite_unselected);
                Assert.IsNotNull(activeSprite);
                Assert.IsNotNull(inactiveSprite);

                // Instantiate panel entry
                UIToolboxPanelEntry newPanelEntry = Instantiate(this.UIToolboxPanelEntryPrefab, this.transform);
                Assert.IsNotNull(newPanelEntry);

                // Set up panel entry
                newPanelEntry.Name = panelEntryConfig.panel_name;
                newPanelEntry.SetSprites(activeSprite, inactiveSprite);
            }

            // Finalise panel
            this.Built = true;
        }

        /// <summary>
        /// Build this panel as a sub panel, populated with component entries
        /// </summary>
        /// <param name="subPanelConfig"></param>
        public void Build(UIToolboxPanelConfig subPanelConfig)
        {
            // Sanity check
            Assert.IsFalse(this.Built);
            Assert.IsNotNull(subPanelConfig);
            Assert.IsNotNull(this.UIToolboxComponentPrefab);

            // Set name
            this.Name = subPanelConfig.panel_name;

            Debug.Log("Building " + this.Name + " as a sub panel");
            foreach (var componentEntryConfig in subPanelConfig.components)
            {
                // Load component prefab
                UILogicComponent componentPrefab = Resources.Load<UILogicComponent>(componentEntryConfig.prefab);
                Assert.IsNotNull(componentPrefab);

                // Load sprites
                Sprite activeSprite = Resources.Load<Sprite>(componentEntryConfig.sprite_selected);
                Sprite inactiveSprite = Resources.Load<Sprite>(componentEntryConfig.sprite_unselected);
                Assert.IsNotNull(activeSprite);
                Assert.IsNotNull(inactiveSprite);

                // Instantiate component entry
                UIToolboxComponent newComponentEntry = Instantiate(this.UIToolboxComponentPrefab, this.transform);
                Assert.IsNotNull(newComponentEntry);

                // Set up component entry
                newComponentEntry.Name = componentEntryConfig.component_name;
                newComponentEntry.SetSprites(activeSprite, inactiveSprite);
                newComponentEntry.LogicComponentPrefab = componentPrefab;
            }

            // Finalise panel
            this.Built = true;
        }

        public void Show()
        {
            this.gameObject.SetActive(true);
            foreach (var entry in this.Entries) entry.gameObject.SetActive(true);
        }

        public void Hide()
        {
            this.gameObject.SetActive(false);
            foreach (var entry in this.Entries) entry.gameObject.SetActive(false);
        }

        public void ToggleSelectedEntry(UIToolboxEntry entrySelection)
        {
            Assert.IsTrue(this.Entries.Contains(entrySelection));
            this.CurrentlySelectedEntry = (this.CurrentlySelectedEntry == entrySelection) ? null : entrySelection;
            foreach (var component in this.Entries)
            {
                component.gameObject.GetComponent<Image>().sprite = (component == this.CurrentlySelectedEntry) ? component.ActiveSprite : component.InactiveSprite;
            }
        }

        /// <summary>
        /// Unselects any currently selected panel entry
        /// </summary>
        public void ResetSelectedEntry()
        {
            if (this.CurrentlySelectedEntry != null)
            {
                this.CurrentlySelectedEntry.gameObject.GetComponent<Image>().sprite = this.CurrentlySelectedEntry.InactiveSprite;
                this.CurrentlySelectedEntry = null;
            }
        }
    }
}
