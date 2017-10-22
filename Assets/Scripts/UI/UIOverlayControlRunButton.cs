using Assets.Scripts.ScratchPad;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public enum UIOverlayControlRunButtonState
    {
        RunButton,
        PauseButton
    }

    public class UIOverlayControlRunButton : MonoBehaviour, IInfoPanelTextProvider, IPointerEnterHandler, IPointerExitHandler
    {
        private SPCanvas Canvas;

        private UIOverlayInfoPanel InfoPanel;

        public Sprite RunButtonSprite;
        public Sprite PauseButtonSprite;

        private const string RUN_BUTTON_INFO_PANEL_TITLE = "Run";
        private const string RUN_BUTTON_INFO_PANEL_DESCRIPTION = "Run the circuit simulation until paused.";
        private const string PAUSE_BUTTON_INFO_PANEL_TITLE = "Pause";
        private const string PAUSE_BUTTON_INFO_PANEL_DESCRIPTION = "Pause the currently running circuit simulation.";

        public UIOverlayControlRunButtonState ButtonState
        {
            get;
            private set;
        }

        public string InfoPanelTitle
        {
            get
            {
                return ButtonState == UIOverlayControlRunButtonState.RunButton ? RUN_BUTTON_INFO_PANEL_TITLE : PAUSE_BUTTON_INFO_PANEL_TITLE;
            }
        }

        public string InfoPanelText
        {
            get
            {
                return ButtonState == UIOverlayControlRunButtonState.RunButton ? RUN_BUTTON_INFO_PANEL_DESCRIPTION : PAUSE_BUTTON_INFO_PANEL_DESCRIPTION;
            }
        }

        /// <summary>
        /// Toggles running/stopped.
        /// Linked to button script in Unity inspector
        /// </summary>
        public void OnButtonClick()
        {
            if (ButtonState == UIOverlayControlRunButtonState.PauseButton)
            {
                PauseButtonClick();
            }
            else
            {
                RunButtonClick();
            }
        }

        private void Awake()
        {
            //
        }

        // Used by the Canvas to update the button's state to reflect
        // whether the canvas is running.
        public void SetButtonStateToNotRunning()
        {
            ButtonState = UIOverlayControlRunButtonState.RunButton;
            gameObject.GetComponent<Image>().sprite = RunButtonSprite;
            InfoPanel.SetInfoTarget(this);

            // Update canvas state
            Canvas.Running = false;
        }

        public void SetButtonStateToRunning()
        {
            ButtonState = UIOverlayControlRunButtonState.PauseButton;
            gameObject.GetComponent<Image>().sprite = PauseButtonSprite;
            InfoPanel.SetInfoTarget(this);
        }

        private void PauseButtonClick()
        {
            // Update canvas state
            Canvas.StopRunning();
        }

        private void RunButtonClick()
        {
            // Update canvas state
            Canvas.Run();
        }

        private void Start()
        {
            Canvas = FindObjectOfType<SPCanvas>();
            Assert.IsNotNull(Canvas);
            InfoPanel = FindObjectOfType<UIOverlayInfoPanel>();
            Assert.IsNotNull(InfoPanel);
            SetButtonStateToNotRunning();
        }

        private void Update()
        {
            //
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
