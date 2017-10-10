using Assets.Scripts.Util;
using System;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace Assets.Scripts.UI.MessageBoxes
{
    public class UIMessageBoxFactory
    {
        private static string UIMessageBoxNamespace = typeof(UIMessageBox).Namespace;

        private GameObject MessageBoxPrefab;

        public UIMessageBoxFactory()
        {
            var o = GameObject.FindObjectOfType<UIController>();
            Assert.IsNotNull(o);
            MessageBoxPrefab = o.UIMessageBoxPrefab;
        }

        private Transform UICanvasTransform
        {
            get
            {
                var o = GameObject.FindObjectOfType<UIController>();
                Assert.IsNotNull(o);
                return o.gameObject.transform;
            }
        }

        /// <summary>
        /// Instantiates and sets up a message box given a config.
        /// </summary>
        /// <param name="config">The configuration parameters for the message box.</param>
        /// <param name="triggerTarget">An object which the message box can trigger a callback in.</param>
        /// <returns></returns>
        public UIMessageBox MakeFromConfig(MessageBoxConfig config, IMessageBoxTriggerTarget triggerTarget = null)
        {
            // Sanity check
            Assert.IsNotNull(config);

            // Build typename from classname
            string fullyQualified = UIMessageBoxNamespace;
            if (config.classname != null) fullyQualified += "." + config.classname;
            Type t = Type.GetType(fullyQualified, throwOnError: true);
            Assert.IsTrue(typeof(UIMessageBox).IsAssignableFrom(t));

            // Instantiate the full prefab onto the UICanvas
            GameObject go = GameObject.Instantiate(
                MessageBoxPrefab,
                UICanvasTransform,
                false);
            Assert.IsNotNull(go);

            // Add the script component to the game object
            UIMessageBox mb = (UIMessageBox)(go.AddComponent(t));

            // Initialise or remove sub-components according to the config
            if (config.title != null)
            {
                var titleText = mb.Title.GetComponent<Text>();
                Assert.IsNotNull(titleText);
                titleText.text = config.title;
            }
            else
            {
                mb.Title.SetActive(false);
            }
            if (config.message != null)
            {
                var messageText = mb.Message.GetComponent<Text>();
                Assert.IsNotNull(messageText);
                messageText.text = config.message;
            }
            else
            {
                mb.Message.SetActive(false);
            }
            if (!config.is_modal)
            {
                mb.BackgroundShade.SetActive(false);
            }

            Assert.IsNotNull(config.text_input); // i think jsonutility default constructs missing things :angery:
            if (!config.text_input.hidden)
            {
                var placeholderText = mb.TextInput.FindChildGameObject("UIMessageBoxTextInputPlaceholder").GetComponent<Text>();
                Assert.IsNotNull(placeholderText);
                placeholderText.text = config.text_input.placeholder ?? "";
            }
            else
            {
                mb.TextInput.SetActive(false);
            }

            Assert.IsNotNull(config.buttons);
            if (!config.buttons.hidden)
            {
                if (!config.buttons.positive_button.hidden)
                {
                    var positiveButtonText = mb.PositiveButton.FindChildGameObject("UIMessageBoxPositiveButtonText").GetComponent<Text>();
                    Assert.IsNotNull(positiveButtonText);
                    positiveButtonText.text = config.buttons.positive_button.label ?? "";

                    // Set the configured onclick method
                    var handler = t.GetMethod(
                        config.buttons.positive_button.onclick,
                        types: Type.EmptyTypes);
                    Assert.IsNotNull(handler);
                    mb.PositiveButton.GetComponent<Button>().onClick.AddListener(() => handler.Invoke(mb, null));
                }
                else
                {
                    mb.PositiveButton.SetActive(false);
                }
                if (!config.buttons.neutral_button.hidden)
                {
                    var neutralButtonText = mb.NeutralButton.FindChildGameObject("UIMessageBoxNeutralButtonText").GetComponent<Text>();
                    Assert.IsNotNull(neutralButtonText);
                    neutralButtonText.text = config.buttons.neutral_button.label ?? "";

                    // Set the configured onclick method
                    var handler = t.GetMethod(
                        config.buttons.neutral_button.onclick,
                        types: Type.EmptyTypes);
                    Assert.IsNotNull(handler);
                    mb.NeutralButton.GetComponent<Button>().onClick.AddListener(() => handler.Invoke(mb, null));
                }
                else
                {
                    mb.NeutralButton.SetActive(false);
                }
                if (!config.buttons.negative_button.hidden)
                {
                    var negativeButtonText = mb.NegativeButton.FindChildGameObject("UIMessageBoxNegativeButtonText").GetComponent<Text>();
                    Assert.IsNotNull(negativeButtonText);
                    negativeButtonText.text = config.buttons.negative_button.label ?? "";

                    // Set the configured onclick method
                    var handler = t.GetMethod(
                        config.buttons.negative_button.onclick,
                        types: Type.EmptyTypes);
                    Assert.IsNotNull(handler);
                    mb.NegativeButton.GetComponent<Button>().onClick.AddListener(() => handler.Invoke(mb, null));
                }
                else
                {
                    mb.NegativeButton.SetActive(false);
                }
            }
            else
            {
                go.FindChildGameObject("UIMessageBoxButtonContainer").SetActive(false);
            }

            // Attach the trigger target
            mb.TriggerTarget = triggerTarget;

            return mb;
        }
    }
}
