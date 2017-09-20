using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;

namespace Assets.Scripts.UI
{
    /// <summary>
    /// An entry in a toolbox panel
    /// </summary>
    public abstract class UIToolboxEntry : MonoBehaviour, IPointerClickHandler
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

        protected void Start()
        {
            this.ToolboxPanel = this.GetComponentInParent<UIToolboxPanel>();
            Assert.IsNotNull(this.ToolboxPanel);
        }

        public void SetSprites(Sprite activeSprite, Sprite inactiveSprite)
        {
            this.ActiveSprite = activeSprite;
            this.InactiveSprite = inactiveSprite;
        }

        public abstract void OnPointerClick(PointerEventData eventData);
    }
}
