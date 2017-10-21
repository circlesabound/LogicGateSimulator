using Assets.Scripts.ScratchPad;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace Assets.Scripts.Savefile
{
    public class SPLogicComponentFactory
    {
        private const string COMPONENT_LIST_RESOURCE = "Configs/components";

        private static string SPLogicComponentNamespace = typeof(SPLogicComponent).Namespace;

        private GameObject CanvasForeground;

        private Dictionary<Type, GameObject> PrefabMapping;
        private Dictionary<Type, Tuple<string, string>> InfoPanelContentMapping;

        public SPLogicComponentFactory(GameObject canvasForeground)
        {
            Assert.IsNotNull(canvasForeground);
            this.CanvasForeground = canvasForeground;

            // Load component list from JSON
            TextAsset componentListAsset = Resources.Load<TextAsset>(COMPONENT_LIST_RESOURCE);
            Assert.IsNotNull(componentListAsset);
            ComponentList componentList = JsonUtility.FromJson<ComponentList>(componentListAsset.text);

            // Populate mapping dictionaries
            PrefabMapping = new Dictionary<Type, GameObject>();
            InfoPanelContentMapping = new Dictionary<Type, Tuple<string, string>>();
            foreach (var componentListElement in componentList.components)
            {
                // Class names in the config file are not fully qualified, we need to append the namespace
                string fullyQualified = SPLogicComponentNamespace + "." + componentListElement.classname;
                Type componentType = Type.GetType(fullyQualified, throwOnError: true);
                Assert.IsTrue(typeof(SPLogicComponent).IsAssignableFrom(componentType));

                // Pre-load the related prefab
                GameObject prefab = Resources.Load<GameObject>(componentListElement.prefab);
                Assert.IsNotNull(prefab);

                Assert.IsFalse(this.PrefabMapping.ContainsKey(componentType));
                this.PrefabMapping.Add(componentType, prefab);

                // Save the base info panel strings
                Assert.IsFalse(InfoPanelContentMapping.ContainsKey(componentType));
                InfoPanelContentMapping.Add(componentType, Tuple.Create(componentListElement.name, componentListElement.description));
            }
            Debug.Log("Populated component prefab map with " + PrefabMapping.Count.ToString() + " pairs.");
        }

        public SPLogicComponent MakeFromConfig(LogicComponentConfig config)
        {
            // Sanity check
            Assert.IsNotNull(config);

            // Build typename from classname
            string fullyQualified = SPLogicComponentNamespace + "." + config.classname;

            // Load the prefab given the typename
            Type t = Type.GetType(fullyQualified, throwOnError: true);
            Assert.IsTrue(typeof(SPLogicComponent).IsAssignableFrom(t));
            Assert.IsTrue(this.PrefabMapping.ContainsKey(t));
            GameObject prefab = this.PrefabMapping[t];

            // Instantiate the component onto the foreground
            GameObject newGameObject = GameObject.Instantiate(
                prefab,
                new Vector3(config.position[0], config.position[1]),
                Quaternion.identity,
                this.CanvasForeground.transform);
            Assert.IsNotNull(newGameObject);
            newGameObject.tag = "SPElement";
            var logicComponent = (SPLogicComponent)(newGameObject.GetComponent(t));

            // Set the base info panel strings
            logicComponent.InfoPanelTitle = InfoPanelContentMapping[t].Item1;
            logicComponent.InfoPanelText = InfoPanelContentMapping[t].Item2;

            return logicComponent;
        }

        #region Component list serialisation data types

#pragma warning disable 0649

        [Serializable]
        private class ComponentList
        {
            public List<ComponentListElement> components;
        }

        [Serializable]
        private class ComponentListElement
        {
            public string classname;
            public string prefab;
            public string name;
            public string description;
        }

#pragma warning restore 0649

        #endregion Component list serialisation data types
    }
}
