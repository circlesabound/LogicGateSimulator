using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    /// <summary>
    /// An entry in a toolbox panel
    /// </summary>
    public abstract class UIToolboxEntry : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler, IInfoPanelTextProvider, IPointerEnterHandler, IPointerExitHandler
    {
        private UIOverlayInfoPanel InfoPanel;
        protected UIToolboxPanel ToolboxPanel;
        private string _InfoPanelName;
        private string _InfoPanelText;

        public Sprite ActiveSprite
        {
            get;
            private set;
        }

        public Sprite InactiveSprite
        {
            get;
            private set;
        }

        public string InfoPanelTitle
        {
            get
            {
                return _InfoPanelName;
            }
            set
            {
                _InfoPanelName = value;
            }
        }

        public string InfoPanelText
        {
            get
            {
                return _InfoPanelText;
            }
            set
            {
                _InfoPanelText = value;
            }
        }

        public virtual void OnPointerClick(PointerEventData eventData)
        {
            this.ToolboxPanel.ToggleSelectedEntry(this);
        }

        public virtual void OnPointerDown(PointerEventData eventData)
        {
            this.ToolboxPanel.ToggleSelectedEntry(this);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            InfoPanel.SetInfoTarget(this);
            InfoPanel.Show();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            InfoPanel.Hide();
        }

        public virtual void OnPointerUp(PointerEventData eventData)
        {
            this.ToolboxPanel.ToggleSelectedEntry(this);
        }

        public void SetSprites(Sprite activeSprite, Sprite inactiveSprite)
        {
            this.ActiveSprite = activeSprite;
            this.InactiveSprite = inactiveSprite;
        }

        protected void Start()
        {
            this.ToolboxPanel = this.GetComponentInParent<UIToolboxPanel>();
            this.gameObject.GetComponent<Image>().sprite = this.InactiveSprite;
            Assert.IsNotNull(this.ToolboxPanel);
            InfoPanel = FindObjectOfType<UIOverlayInfoPanel>();
            Assert.IsNotNull(InfoPanel);
        }
    }
}
