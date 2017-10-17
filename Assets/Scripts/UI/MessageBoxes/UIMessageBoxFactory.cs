using Assets.Scripts.Util;
using System;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts.UI.MessageBoxes
{
    public class UIMessageBoxFactory
    {
        private static string UIMessageBoxNamespace = typeof(UIMessageBox).Namespace;

        private GameObject MessageBoxPrefab;
        private GameObject MessageBoxScrollViewItemPrefab;

        public UIMessageBoxFactory()
        {
            var o = GameObject.FindObjectOfType<UIController>();
            Assert.IsNotNull(o);
            MessageBoxPrefab = o.UIMessageBoxPrefab;
            MessageBoxScrollViewItemPrefab = o.UIMessageBoxScrollViewItemPrefab;
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
            // Be warned: horrible reflection code below
            // Might as well be using a dynamically typed language

            // Sanity check
            Assert.IsNotNull(config);

            // Build typename from classname
            string fullyQualified = UIMessageBoxNamespace + "." + config.classname;
            Type messageBoxType = Type.GetType(fullyQualified, throwOnError: true);
            Assert.IsTrue(typeof(UIMessageBox).IsAssignableFrom(messageBoxType));

            // Instantiate the full prefab onto the UICanvas
            GameObject go = GameObject.Instantiate(
                MessageBoxPrefab,
                UICanvasTransform,
                false);
            Assert.IsNotNull(go);

            // Add the script component to the game object
            UIMessageBox mb = (UIMessageBox)(go.AddComponent(messageBoxType));

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

            Assert.IsNotNull(config.scroll_view); // i think jsonutility default constructs missing things :angery:
            if (!config.scroll_view.hidden)
            {
                Assert.IsTrue(config.scroll_view.content_type == "static" || config.scroll_view.content_type == "dynamic");
                if (config.scroll_view.content_type == "static")
                {
                    Assert.IsNotNull(config.scroll_view.static_content);
                    Assert.IsNotNull(config.scroll_view.static_content.items);
                    foreach (var itemConfig in config.scroll_view.static_content.items)
                    {
                        // Build typename from classname
                        string itemClassname = UIMessageBoxNamespace + "." + itemConfig.classname;
                        Type itemType = Type.GetType(itemClassname, throwOnError: true);
                        Assert.IsTrue(typeof(MessageBoxScrollViewItem).IsAssignableFrom(itemType));

                        // Instantiate a scrollview item from the prefab
                        GameObject itemObject = GameObject.Instantiate(
                            MessageBoxScrollViewItemPrefab,
                            mb.ScrollView.transform,
                            false);
                        Assert.IsNotNull(itemObject);

                        // Add the script component
                        var item = (MessageBoxScrollViewItem)(itemObject.AddComponent(itemType));

                        // Set the configured onclick method
                        var handler = itemType.GetMethod(
                            itemConfig.onclick,
                            types: new[] { typeof(PointerEventData) });
                        Assert.IsNotNull(handler);
                        var eventEntry = new EventTrigger.Entry
                        {
                            eventID = EventTriggerType.PointerClick
                        };
                        eventEntry.callback.AddListener(eventData => handler.Invoke(item, new[] { (PointerEventData)eventData }));
                        itemObject.GetComponent<EventTrigger>().triggers.Add(eventEntry);

                        // Set the item text
                        var itemText = item.Text.GetComponent<Text>();
                        Assert.IsNotNull(itemText);
                        itemText.text = itemConfig.label;
                    }
                }
                else if (config.scroll_view.content_type == "dynamic")
                {
                    Assert.IsNotNull(config.scroll_view.dynamic_content);
                    Assert.IsTrue(typeof(IScrollViewItemProvider).IsAssignableFrom(messageBoxType));

                    // Build typename from classname
                    string itemClassName = UIMessageBoxNamespace + "." + config.scroll_view.dynamic_content.item_blueprint.classname;
                    Type itemType = Type.GetType(itemClassName, throwOnError: true);
                    Assert.IsTrue(typeof(MessageBoxScrollViewItem).IsAssignableFrom(itemType));

                    // Enumerate over the IEnumerable<string> from the item provider method
                    var provider = (IScrollViewItemProvider)mb;
                    foreach (var itemLabel in provider.GenerateScrollViewItems())
                    {
                        // Instantiate a scrollview item from the prefab
                        GameObject itemObject = GameObject.Instantiate(
                            MessageBoxScrollViewItemPrefab,
                            mb.ScrollView.transform,
                            false);
                        Assert.IsNotNull(itemObject);

                        // Add the script component
                        var item = (MessageBoxScrollViewItem)(itemObject.AddComponent(itemType));

                        // Set the configured onclick method
                        var handler = itemType.GetMethod(
                            config.scroll_view.dynamic_content.item_blueprint.onclick,
                            types: new[] { typeof(PointerEventData) });
                        Assert.IsNotNull(handler);
                        var eventEntry = new EventTrigger.Entry
                        {
                            eventID = EventTriggerType.PointerClick
                        };
                        eventEntry.callback.AddListener(eventData => handler.Invoke(item, new[] { (PointerEventData)eventData }));
                        itemObject.GetComponent<EventTrigger>().triggers.Add(eventEntry);

                        // Set the item text
                        var itemText = item.Text.GetComponent<Text>();
                        Assert.IsNotNull(itemText);
                        itemText.text = itemLabel;
                    }
                }
            }
            else
            {
                go.FindChildGameObject("UIMessageBox/UIMessageBoxScrollViewContainer").SetActive(false);
            }

            Assert.IsNotNull(config.text_input);
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

            Assert.IsNotNull(config.number_slider);
            if (!config.number_slider.hidden)
            {
                var slider = mb.SliderContainer.FindChildGameObject("UIMessageBoxNumberSlider").GetComponent<Slider>();
                Assert.IsNotNull(slider);
                slider.minValue = config.number_slider.min_value;
                slider.maxValue = config.number_slider.max_value;

                // Set up labels
                Action<MessageBoxConfig.MessageBoxNumberSliderLabelConfig, GameObject, string> labelSetup = (labelConfig, labelObject, dictKey) =>
                {
                    if (!labelConfig.hidden)
                    {
                        if (labelConfig.content_type == "static")
                        {
                            mb.NumberSliderLabelGenerators["left"] = () => labelConfig.static_content.label;
                        }
                        else if (labelConfig.content_type == "dynamic")
                        {
                            // Build provider typename from classname
                            string providerClassname = UIMessageBoxNamespace + "." + labelConfig.dynamic_provider.classname;
                            Type providerType = Type.GetType(providerClassname, throwOnError: true);
                            Assert.IsTrue(typeof(ILabelProvider).IsAssignableFrom(providerType));
                            Assert.IsTrue(typeof(MonoBehaviour).IsAssignableFrom(providerType));

                            // Add a provider instance as a script component
                            var labelProvider = (ILabelProvider)labelObject.AddComponent(providerType);

                            // Set up label generator
                            mb.NumberSliderLabelGenerators[dictKey] = labelProvider.GenerateLabel;
                        }
                    }
                    else
                    {
                        labelObject.SetActive(false);
                    }
                };
                labelSetup(
                    config.number_slider.label_left,
                    mb.SliderContainer.FindChildGameObject("UIMessageBoxNumberSliderLabelLeft"),
                    "left");
                labelSetup(
                    config.number_slider.label_right,
                    mb.SliderContainer.FindChildGameObject("UIMessageBoxNumberSliderLabelRight"),
                    "right");
            }
            else
            {
                mb.SliderContainer.SetActive(false);
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
                    var handler = messageBoxType.GetMethod(
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
                    var handler = messageBoxType.GetMethod(
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
                    var handler = messageBoxType.GetMethod(
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
