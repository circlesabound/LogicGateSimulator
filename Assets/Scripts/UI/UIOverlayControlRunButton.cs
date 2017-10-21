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

        public Sprite RunButtonSprite;
        public Sprite PauseButtonSprite;

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
            gameObject.GetComponent<Image>().sprite = RunButtonSprite;

            // Update canvas state
            Canvas.Running = false;
        }

        public void SetButtonStateToRunning()
        {
            ButtonState = UIOverlayControlRunButtonState.PauseButton;
            gameObject.GetComponent<Image>().sprite = PauseButtonSprite;
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
            SetButtonStateToNotRunning();
        }

        private void Update()
        {
            //
        }
    }
}
