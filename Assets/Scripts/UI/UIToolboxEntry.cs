using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;

namespace Assets.Scripts.UI
{
    /// <summary>
    /// An entry in a toolbox panel
    /// </summary>
    public abstract class UIToolboxEntry : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
    {
        protected UIToolboxPanel ToolboxPanel;

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

        public virtual void OnPointerClick(PointerEventData eventData)
        {
            this.ToolboxPanel.ToggleSelectedEntry(this);
        }

        public virtual void OnPointerDown(PointerEventData eventData)
        {
            this.ToolboxPanel.ToggleSelectedEntry(this);
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
            Assert.IsNotNull(this.ToolboxPanel);
        }
    }
}
