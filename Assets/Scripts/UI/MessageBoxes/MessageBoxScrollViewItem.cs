using Assets.Scripts.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.UI.MessageBoxes
{
    public abstract class MessageBoxScrollViewItem : MonoBehaviour
    {
        public GameObject Text
        {
            get
            {
                return this.gameObject.FindChildGameObject("UIMessageBoxScrollViewItemText");
            }
        }
    }
}
