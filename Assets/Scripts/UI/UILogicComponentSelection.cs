using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    /// <summary>
    /// An element on the logic component selection panel
    /// </summary>
    public class UILogicComponentSelection : MonoBehaviour, IPointerClickHandler
    {
        private UILogicComponentSelectionPanel ComponentSelectionPanel;
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
        public UILogicComponent LogicComponentPrefab;

        // Use this for initialization
        void Start()
        {
            this.ComponentSelectionPanel = this.GetComponentInParent<UILogicComponentSelectionPanel>();
            Assert.IsNotNull(this.ComponentSelectionPanel);
        }

        // Update is called once per frame
        void Update()
        {
            //
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            this.ComponentSelectionPanel.ToggleSelectedComponent(this);
        }

        public void SetSprites(Sprite activeSprite, Sprite inactiveSprite)
        {
            this.ActiveSprite = activeSprite;
            this.InactiveSprite = inactiveSprite;
        }
    }
}
