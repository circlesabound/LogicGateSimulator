using Assets.Scripts.ScratchPad;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public enum UIOverlayControlRunButtonState
    {
        RunButton,
        PauseButton
    }

    public class UIOverlayControlRunButton : MonoBehaviour
    {
        private SPCanvas Canvas;
        private Text DisplayText;

        public UIOverlayControlRunButtonState ButtonState
        {
            get;
            private set;
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
            DisplayText.text = "Run";
        }

        public void SetButtonStateToRunning()
        {
            ButtonState = UIOverlayControlRunButtonState.PauseButton;
            DisplayText.text = "Pause";
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
            var displayTextTransform = this.gameObject.transform.Find("UIOverlayControlRunButtonText");
            Assert.IsNotNull(displayTextTransform);
            DisplayText = displayTextTransform.gameObject.GetComponent<Text>();
            Assert.IsNotNull(DisplayText);
            SetButtonStateToNotRunning();
        }

        private void Update()
        {
            //
        }
    }
}
