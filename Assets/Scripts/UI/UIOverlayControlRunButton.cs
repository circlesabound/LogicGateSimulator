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
                SetNotRunning();
            }
            else
            {
                SetRunning();
            }
        }

        private void Awake()
        {
            //
        }

        private void SetNotRunning()
        {
            // Swap the button
            ButtonState = UIOverlayControlRunButtonState.RunButton;
            DisplayText.text = "Run";
            gameObject.GetComponent<Image>().sprite = RunButtonSprite;

            // Update canvas state
            Canvas.Running = false;
        }

        private void SetRunning()
        {
            // Swap the button
            ButtonState = UIOverlayControlRunButtonState.PauseButton;
            DisplayText.text = "Pause";
            gameObject.GetComponent<Image>().sprite = PauseButtonSprite;

            // Update canvas state
            Canvas.Running = true;
        }

        private void Start()
        {
            Canvas = FindObjectOfType<SPCanvas>();
            Assert.IsNotNull(Canvas);
            var displayTextTransform = this.gameObject.transform.Find("UIOverlayControlRunButtonText");
            Assert.IsNotNull(displayTextTransform);
            DisplayText = displayTextTransform.gameObject.GetComponent<Text>();
            Assert.IsNotNull(DisplayText);
            SetNotRunning();
        }

        private void Update()
        {
            //
        }
    }
}
