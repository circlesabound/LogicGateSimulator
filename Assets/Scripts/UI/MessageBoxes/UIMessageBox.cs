using Assets.Scripts.Util;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.MessageBoxes
{
    public abstract class UIMessageBox : MonoBehaviour, IMessageBoxTriggerTarget
    {
        public IMessageBoxTriggerTarget TriggerTarget;

        public enum MessageBoxButtonType
        {
            Positive,
            Neutral,
            Negative
        }

        public Dictionary<string, Func<string>> NumberSliderLabelGenerators = new Dictionary<string, Func<string>>
        {
            { "left", null },
            { "right", null }
        };

        public GameObject BackgroundShade
        {
            get
            {
                return gameObject.FindChildGameObject("UIMessageBoxBackgroundShade");
            }
        }

        public GameObject Message
        {
            get
            {
                return gameObject.FindChildGameObject("UIMessageBox/UIMessageBoxMessage");
            }
        }

        public GameObject MessageBox
        {
            get
            {
                return gameObject.FindChildGameObject("UIMessageBox");
            }
        }

        public GameObject NegativeButton
        {
            get
            {
                return gameObject.FindChildGameObject("UIMessageBox/UIMessageBoxButtonContainer/UIMessageBoxNegativeButton");
            }
        }

        public GameObject NeutralButton
        {
            get
            {
                return gameObject.FindChildGameObject("UIMessageBox/UIMessageBoxButtonContainer/UIMessageBoxNeutralButton");
            }
        }

        public GameObject PositiveButton
        {
            get
            {
                return gameObject.FindChildGameObject("UIMessageBox/UIMessageBoxButtonContainer/UIMessageBoxPositiveButton");
            }
        }

        public GameObject ScrollView
        {
            get
            {
                return gameObject.FindChildGameObject("UIMessageBox/UIMessageBoxScrollViewContainer/UIMessageBoxScrollView/UIMessageBoxScrollViewMask/UIMessageBoxScrollViewContent");
            }
        }

        public GameObject SliderContainer
        {
            get
            {
                return gameObject.FindChildGameObject("UIMessageBox/UIMessageBoxNumberSliderContainer");
            }
        }

        public GameObject TextInput
        {
            get
            {
                return gameObject.FindChildGameObject("UIMessageBox/UIMessageBoxTextInput");
            }
        }

        public GameObject Title
        {
            get
            {
                return gameObject.FindChildGameObject("UIMessageBox/UIMessageBoxTitle");
            }
        }

        public abstract void Trigger(MessageBoxTriggerData triggerData);

        public virtual void Update()
        {
            if (NumberSliderLabelGenerators["left"] != null)
            {
                var leftLabel = SliderContainer.FindChildGameObject("UIMessageBoxNumberSliderLabelLeft");
                leftLabel.GetComponent<Text>().text = NumberSliderLabelGenerators["left"]();
            }
            if (NumberSliderLabelGenerators["right"] != null)
            {
                var rightLabel = SliderContainer.FindChildGameObject("UIMessageBoxNumberSliderLabelRight");
                rightLabel.GetComponent<Text>().text = NumberSliderLabelGenerators["right"]();
            }
        }
    }
}
