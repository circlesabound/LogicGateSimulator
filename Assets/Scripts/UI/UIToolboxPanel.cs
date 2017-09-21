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
        public const string NAME_PREFIX = "UIToolboxPanel_";

        public UIToolboxPanelEntry UIToolboxPanelEntryPrefab;
        public UIToolboxComponent UIToolboxComponentPrefab;

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

        private List<UIToolboxEntry> Entries;
        private UIToolboxEntry CurrentlySelectedEntry;
        private bool Built;

        public bool IsMainPanel
        {
            get
            {
                return this.SimpleName == "Main";
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
        /// Build this panel as a main panel, populated with sub panel entries
        /// </summary>
        /// <param name="mainPanelConfig"></param>
        public void Build(UIToolboxConfig mainPanelConfig)
        {
            // Sanity check
            Assert.IsFalse(this.Built);
            Assert.IsNotNull(mainPanelConfig);
            Assert.IsNotNull(this.UIToolboxPanelEntryPrefab);

            // Set name
            this.SimpleName = "Main";

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
                this.Entries.Add(newPanelEntry);

                // Set up panel entry
                newPanelEntry.SimpleName = panelEntryConfig.panel_name;
                newPanelEntry.SetSprites(activeSprite, inactiveSprite);
            }

            // Finalise panel
            this.Built = true;
            Debug.Log("Built '" + this.name + "' as a main panel with " + this.Entries.Count + " entries");
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
            this.SimpleName = subPanelConfig.panel_name;

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
                this.Entries.Add(newComponentEntry);

                // Set up component entry
                newComponentEntry.SimpleName = componentEntryConfig.component_name;
                newComponentEntry.SetSprites(activeSprite, inactiveSprite);
                newComponentEntry.LogicComponentPrefab = componentPrefab;
            }

            // Finalise panel
            this.Built = true;
            Debug.Log("Built '" + this.name + "' as a sub panel with " + this.Entries.Count + " entries");
        }

        public void Show()
        {
            CanvasGroup canvasGroup = this.GetComponent<CanvasGroup>();
            Assert.IsNotNull(canvasGroup);
            canvasGroup.interactable = true;
            canvasGroup.alpha = 1;
            canvasGroup.blocksRaycasts = true;
        }

        public void Hide()
        {
            CanvasGroup canvasGroup = this.GetComponent<CanvasGroup>();
            Assert.IsNotNull(canvasGroup);
            canvasGroup.interactable = false;
            canvasGroup.alpha = 0;
            canvasGroup.blocksRaycasts = false;
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
