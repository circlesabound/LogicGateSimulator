using Assets.Scripts.ScratchPad;
using System;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;

namespace Assets.Scripts.UI
{
    public class UIOverlayControlResetButton : MonoBehaviour, IInfoPanelTextProvider, IPointerEnterHandler, IPointerExitHandler
    {
        private SPCanvas Canvas;
        private UIOverlayInfoPanel InfoPanel;

        private const string RESET_BUTTON_INFO_PANEL_TITLE = "Reset circuit";
        private const string RESET_BUTTON_INFO_PANEL_DESCRIPTION = "Reset the values of all circuit components.";

        public string InfoPanelTitle
        {
            get
            {
                return RESET_BUTTON_INFO_PANEL_TITLE;
            }
        }

        public string InfoPanelText
        {
            get
            {
                return RESET_BUTTON_INFO_PANEL_DESCRIPTION;
            }
        }

        /// <summary>
        /// Linked to button click in Unity inspector
        /// </summary>
        public void OnButtonClick()
        {
            Canvas.ResetCircuit();
        }

        private void Start()
        {
            Canvas = FindObjectOfType<SPCanvas>();
            Assert.IsNotNull(Canvas);
            InfoPanel = FindObjectOfType<UIOverlayInfoPanel>();
            Assert.IsNotNull(InfoPanel);
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
    }
}
