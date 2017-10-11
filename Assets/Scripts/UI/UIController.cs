using Assets.Scripts.UI.MessageBoxes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
