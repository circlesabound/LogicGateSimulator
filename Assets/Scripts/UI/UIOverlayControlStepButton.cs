using Assets.Scripts.ScratchPad;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;

namespace Assets.Scripts.UI
{
    public class UIOverlayControlStepButton : MonoBehaviour, IInfoPanelTextProvider, IPointerEnterHandler, IPointerExitHandler
    {
        private SPCanvas Canvas;

        private UIOverlayInfoPanel InfoPanel;

        private const string STEP_BUTTON_INFO_PANEL_TITLE = "Step";
        private const string STEP_BUTTON_INFO_PANEL_DESCRIPTION = "Run the circuit simulation for a single step.";

        public string InfoPanelTitle
        {
            get
            {
                return STEP_BUTTON_INFO_PANEL_TITLE;
            }
        }

        public string InfoPanelText
        {
            get
            {
                return STEP_BUTTON_INFO_PANEL_DESCRIPTION;
            }
        }

        /// <summary>
        /// Linked to button click in Unity inspector
        /// </summary>
        public void OnButtonClick()
        {
            Canvas.Circuit.Simulate();
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
