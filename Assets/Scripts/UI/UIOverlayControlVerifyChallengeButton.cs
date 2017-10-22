using Assets.Scripts.Savefile;
using Assets.Scripts.ScratchPad;
using Assets.Scripts.UI.MessageBoxes;
using System;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;

namespace Assets.Scripts.UI
{
    public class UIOverlayControlVerifyChallengeButton : MonoBehaviour, IMessageBoxTriggerTarget, IInfoPanelTextProvider, IPointerEnterHandler, IPointerExitHandler
    {
        private SPCanvas Canvas;
        private UIOverlayInfoPanel InfoPanel;

        private UIMessageBoxFactory MessageBoxFactory;
        private MessageBoxConfig NotInChallengeModeMessageBoxConfig;
        private MessageBoxConfig ChallengeCompleteConfig;
        private MessageBoxConfig ActivateAllOutputs_IncompleteMessageBoxConfig;
        private MessageBoxConfig MatchTestCases_IncompleteMessageBoxConfig;

        private const string NOT_IN_CHALLENGE_MODE_CONFIG_RESOURCE = "Configs/MessageBoxes/not_in_challenge_mode";
        private const string CHALLENGE_COMPLETE_CONFIG_RESOURCE = "Configs/MessageBoxes/challenge_complete";
        private const string ACTIVATE_ALL_OUTPUTS_INCOMPLETE_CONFIG_RESOURCE = "Configs/MessageBoxes/challenge_incomplete_activatealloutputs";
        private const string MATCH_TEST_CASES_INCOMPLETE_CONFIG_RESOURCE = "Configs/MessageBoxes/challenge_incomplete_matchtestcases";

        private const string VERIFY_BUTTON_INFO_PANEL_TITLE = "Verify challenge";
        private const string VERIFY_BUTTON_INFO_PANEL_DESCRIPTION = "Verifies the challenge has been completed. Only available in Challenge mode.";

        public string InfoPanelTitle
        {
            get
            {
                return VERIFY_BUTTON_INFO_PANEL_TITLE;
            }
        }

        public string InfoPanelText
        {
            get
            {
                return VERIFY_BUTTON_INFO_PANEL_DESCRIPTION;
            }
        }

        /// <summary>
        /// Linked to button click in Unity inspector
        /// </summary>
        public void OnButtonClick()
        {
            Canvas.Frozen = true;
            //TODO:
            if (Canvas.CurrentMode == GameMode.Sandbox)
            {
                MessageBoxFactory.MakeFromConfig(NotInChallengeModeMessageBoxConfig, this);
            }
            else
            {
                if (Canvas.CurrentMode == GameMode.ActivateAllOutputsChallenge)
                {
                    bool challengeComplete = Canvas.Circuit.Validate();
                    if (challengeComplete)
                    {
                        MessageBoxFactory.MakeFromConfig(ChallengeCompleteConfig, this);
                    }
                    else
                    {
                        MessageBoxFactory.MakeFromConfig(ActivateAllOutputs_IncompleteMessageBoxConfig, this);
                    }
                }
                if (Canvas.CurrentMode == GameMode.MatchTestcasesChallenge)
                {
                    bool challengeComplete = true;
                    foreach (TestCaseConfig testCase in Canvas.TestCases)
                    {
                        if (!challengeComplete)
                        {
                            break;
                        }
                        challengeComplete = Canvas.Circuit.ValidateTestCase(testCase.GetAllInputs(),
                                                                            testCase.GetAllOutputs(),
                                                                            SPCanvas.TestCaseStepsRun);
                    }
                    Canvas.Circuit.ResetComponents();
                    if (challengeComplete)
                    {
                        MessageBoxFactory.MakeFromConfig(ChallengeCompleteConfig, this);
                    }
                    else
                    {
                        MessageBoxFactory.MakeFromConfig(MatchTestCases_IncompleteMessageBoxConfig, this);
                    }
                }
            }
        }

        /// <summary>
        /// Callback to be executed after displaying the message box.
        /// Unfreezes the canvas and closes the message box.
        /// </summary>
        /// <param name="triggerData">Data to pass from the trigger source to the trigger target.</param>
        public void Trigger(MessageBoxTriggerData triggerData)
        {
            Canvas.Frozen = false;
            Destroy(triggerData.Sender.gameObject);
            return;
        }

        private void Awake()
        {
            MessageBoxFactory = new UIMessageBoxFactory();

            // Load the message box config for not in challenge mode
            TextAsset configAsset = Resources.Load<TextAsset>(NOT_IN_CHALLENGE_MODE_CONFIG_RESOURCE);
            Assert.IsNotNull(configAsset);
            NotInChallengeModeMessageBoxConfig = JsonUtility.FromJson<MessageBoxConfig>(configAsset.text);

            // Load the message box config for completing a challenge
            configAsset = Resources.Load<TextAsset>(CHALLENGE_COMPLETE_CONFIG_RESOURCE);
            Assert.IsNotNull(configAsset);
            ChallengeCompleteConfig = JsonUtility.FromJson<MessageBoxConfig>(configAsset.text);

            // Load the message box config for incomplete solution for activate all outputs mode
            configAsset = Resources.Load<TextAsset>(ACTIVATE_ALL_OUTPUTS_INCOMPLETE_CONFIG_RESOURCE);
            Assert.IsNotNull(configAsset);
            ActivateAllOutputs_IncompleteMessageBoxConfig = JsonUtility.FromJson<MessageBoxConfig>(configAsset.text);

            // Load the message box config for incomplete solution for match test cases mode
            configAsset = Resources.Load<TextAsset>(MATCH_TEST_CASES_INCOMPLETE_CONFIG_RESOURCE);
            Assert.IsNotNull(configAsset);
            MatchTestCases_IncompleteMessageBoxConfig = JsonUtility.FromJson<MessageBoxConfig>(configAsset.text);
        }
 

        private void Start()
        {
            Canvas = FindObjectOfType<SPCanvas>();
            Assert.IsNotNull(Canvas);
            InfoPanel = FindObjectOfType<UIOverlayInfoPanel>();
            Assert.IsNotNull(InfoPanel);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            InfoPanel.SetInfoTarget(this);
            InfoPanel.Show();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            InfoPanel.Hide();
        }
    }
}
