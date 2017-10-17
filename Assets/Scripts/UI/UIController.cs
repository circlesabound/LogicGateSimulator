using UnityEngine;
using UnityEngine.Assertions;

namespace Assets.Scripts.UI
{
    public class UIController : MonoBehaviour
    {
        public GameObject UIMessageBoxPrefab;
        public GameObject UIMessageBoxScrollViewItemPrefab;

        private void Awake()
        {
            Assert.IsNotNull(UIMessageBoxPrefab);
            Assert.IsNotNull(UIMessageBoxScrollViewItemPrefab);
        }

        private void Start()
        {
        }
    }
}
