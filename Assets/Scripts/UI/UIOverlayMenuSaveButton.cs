using Assets.Scripts.Savefile;
using Assets.Scripts.ScratchPad;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

namespace Assets.Scripts.UI
{
    public class UIOverlayMenuSaveButton : MonoBehaviour
    {
        private SPCanvas Canvas;

        /// <summary>
        /// Linked to button click in Unity inspector
        /// </summary>
        public void OnButtonClick()
        {
            // Bind a GUID for each component
            Dictionary<SPLogicComponent, Guid> guidMap = Canvas.Components
                .ToDictionary(c => c, c => Guid.NewGuid());

            // Generate configs for components
            List<LogicComponentConfig> componentConfigs = guidMap
                .Select(kvp => kvp.Key.GenerateConfig(kvp.Value))
                .ToList();

            // Generate configs for edges, mapping connected components using GUIDs
            List<EdgeConfig> edgeConfigs = Canvas.Edges
                .Select(e => e.GenerateConfig(guidMap))
                .ToList();

            // Build savefile
            ScratchPadConfig spConfig = new ScratchPadConfig(componentConfigs, edgeConfigs);
#if DEVELOPMENT_BUILD
            string saveString = JsonUtility.ToJson(spConfig, prettyPrint: true);
#else
            string saveString = JsonUtility.ToJson(spConfig, prettyPrint: false);
#endif
            Debug.Log(saveString);
        }

        private void Start()
        {
            Canvas = FindObjectOfType<SPCanvas>();
            Assert.IsNotNull(Canvas);
        }
    }
}
