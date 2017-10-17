using Assets.Scripts.ScratchPad;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    /// <summary>
    /// A panel in the toolbox
    /// </summary>
    public class UIToolboxPanel : MonoBehaviour
    {
        public const string NAME_PREFIX = "UIToolboxPanel_";
        public UIToolboxComponentEntry UIToolboxComponentEntryPrefab;
        public UIToolboxSubpanelEntry UIToolboxSubpanelEntryPrefab;

        private bool Built;

        private List<UIToolboxEntry> Entries;

        public UIToolboxEntry CurrentlySelectedEntry
        {
            get;
            private set;
        }

        public bool IsMainPanel
        {
            get
            {
                return this.Built && this.SimpleName == "Main";
            }
        }

        public string SimpleName
        {
            get
            {
                return this.name.Substring(NAME_PREFIX.Length);
            }
            set
            {
                this.name = NAME_PREFIX + value;
                Transform titleTransform = this.gameObject.transform.Find("UIToolboxPanelHeader/UIToolboxPanelTitle");
                Assert.IsNotNull(titleTransform);
                Text titleText = titleTransform.gameObject.GetComponent<Text>();
                Assert.IsNotNull(titleText);
                titleText.text = value;
            }
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
            Assert.IsNotNull(this.UIToolboxSubpanelEntryPrefab);

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
                UIToolboxSubpanelEntry newPanelEntry = Instantiate(this.UIToolboxSubpanelEntryPrefab, this.transform);
                Assert.IsNotNull(newPanelEntry);
                this.Entries.Add(newPanelEntry);

                // Set up panel entry
                newPanelEntry.SimpleName = panelEntryConfig.panel_name;
                newPanelEntry.SetSprites(activeSprite, inactiveSprite);
            }

            // Remove the back button
            GameObject.Destroy(this.GetComponentInChildren<UIToolboxPanelBackButton>().gameObject);

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
            Assert.IsNotNull(this.UIToolboxComponentEntryPrefab);

            // Set name
            this.SimpleName = subPanelConfig.panel_name;

            foreach (var componentEntryConfig in subPanelConfig.components)
            {
                // Load sprites
                Sprite activeSprite = Resources.Load<Sprite>(componentEntryConfig.sprite_selected);
                Sprite inactiveSprite = Resources.Load<Sprite>(componentEntryConfig.sprite_unselected);
                Assert.IsNotNull(activeSprite);
                Assert.IsNotNull(inactiveSprite);

                // Instantiate component entry
                UIToolboxComponentEntry newComponentEntry = Instantiate(this.UIToolboxComponentEntryPrefab, this.transform);
                Assert.IsNotNull(newComponentEntry);
                this.Entries.Add(newComponentEntry);

                // Set up component entry
                newComponentEntry.SimpleName = componentEntryConfig.component_name;
                newComponentEntry.SetSprites(activeSprite, inactiveSprite);
                newComponentEntry.ComponentClassname = componentEntryConfig.component_classname;
            }

            // Finalise panel
            this.Built = true;
            Debug.Log("Built '" + this.name + "' as a sub panel with " + this.Entries.Count + " entries");
        }

        public void Hide()
        {
            // Reset panel state
            this.ResetSelectedEntry();

            // Hide the panel
            CanvasGroup canvasGroup = this.GetComponent<CanvasGroup>();
            Assert.IsNotNull(canvasGroup);
            canvasGroup.interactable = false;
            canvasGroup.alpha = 0;
            canvasGroup.blocksRaycasts = false;
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

        public void Show()
        {
            CanvasGroup canvasGroup = this.GetComponent<CanvasGroup>();
            Assert.IsNotNull(canvasGroup);
            canvasGroup.interactable = true;
            canvasGroup.alpha = 1;
            canvasGroup.blocksRaycasts = true;
        }

        public void ToggleSelectedEntry(UIToolboxEntry entrySelection)
        {
            Assert.IsTrue(this.Entries.Contains(entrySelection));
            if (this.CurrentlySelectedEntry == entrySelection)
            {
                this.CurrentlySelectedEntry = null;
                GameObject.FindObjectOfType<SPCanvas>().RestorePreviousTool();
            }
            else
            {
                this.CurrentlySelectedEntry = entrySelection;
                GameObject.FindObjectOfType<SPCanvas>().CurrentTool = SPTool.NewComponent;
            }
            foreach (var component in this.Entries)
            {
                component.gameObject.GetComponent<Image>().sprite = (component == this.CurrentlySelectedEntry) ? component.ActiveSprite : component.InactiveSprite;
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
    }
}
