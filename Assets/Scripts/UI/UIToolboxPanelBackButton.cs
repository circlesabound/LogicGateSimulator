using UnityEngine;

namespace Assets.Scripts.UI
{
    public class UIToolboxPanelBackButton : MonoBehaviour
    {
        /// <summary>
        /// Assign behaviour in Unity inspector because it's easier
        /// </summary>
        public void ClickBackButton()
        {
            UIToolboxContainer toolbox = GameObject.FindObjectOfType<UIToolboxContainer>();
            toolbox.CurrentPanel = toolbox.MainPanel;
        }
    }
}
