using Assets.Scripts.Util;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.MessageBoxes
{
    public class TestProvider : MonoBehaviour, ILabelProvider
    {
        public string GenerateLabel()
        {
            return gameObject.transform.parent.gameObject.FindChildGameObject("UIMessageBoxNumberSlider").GetComponent<Slider>().value.ToString();
        }
    }
}
