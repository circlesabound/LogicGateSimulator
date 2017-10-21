using Assets.Scripts.Util;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class UIOverlayInfoPanel : MonoBehaviour
    {
        private Text BodyText
        {
            get
            {
                return gameObject.FindChildGameObject("UIOverlayInfoPanelBodyText").GetComponent<Text>();
            }
        }

        private Text TitleText
        {
            get
            {
                return gameObject.FindChildGameObject("UIOverlayInfoPanelTitleText").GetComponent<Text>();
            }
        }

        public void Hide()
        {
            CanvasGroup canvasGroup = this.GetComponent<CanvasGroup>();
            Assert.IsNotNull(canvasGroup);
            canvasGroup.interactable = false;
            canvasGroup.alpha = 0;
        }

        public void SetInfoTarget(IInfoPanelTextProvider target)
        {
            TitleText.text = target.InfoPanelTitle;
            BodyText.text = target.InfoPanelText;
        }

        public void Show()
        {
            CanvasGroup canvasGroup = this.GetComponent<CanvasGroup>();
            Assert.IsNotNull(canvasGroup);
            canvasGroup.interactable = true;
            canvasGroup.alpha = 1;
        }
    }
}
