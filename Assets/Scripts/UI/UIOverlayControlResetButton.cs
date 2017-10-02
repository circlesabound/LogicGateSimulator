using Assets.Scripts.ScratchPad;
using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace Assets.Scripts.UI
{
    public class UIOverlayControlResetButton : MonoBehaviour
    {
        private SPCanvas Canvas;

        /// <summary>
        /// Linked to button click in Unity inspector
        /// </summary>
        public void OnButtonClick()
        {
            throw new NotImplementedException();
        }

        private void Start()
        {
            Canvas = FindObjectOfType<SPCanvas>();
            Assert.IsNotNull(Canvas);
        }
    }
}
