using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;

namespace Assets.Scripts.UI
{
    public class UILogicComponentSelectionPanel : MonoBehaviour
    {
        public UILogicComponentSelection UILogicComponentSelectionPrefab;
        private List<UILogicComponentSelection> ComponentSelectionList;
        private UILogicComponentSelection CurrentlySelectedComponent;

        // Use this for initialization
        private void Start()
        {
            this.ComponentSelectionList = new List<UILogicComponentSelection>()
            {
                this.InstantiateUILogicComponentSelection("Prefabs/UITrueConst", "Sprites/TestSquareTextureRed", "Sprites/TestSquareTexture"),
                this.InstantiateUILogicComponentSelection("Prefabs/UIFalseConst", "Sprites/TestSquareTextureRed", "Sprites/TestSquareTexture"),
                this.InstantiateUILogicComponentSelection("Prefabs/UIOutput", "Sprites/TestSquareTextureRed", "Sprites/TestSquareTexture"),
                this.InstantiateUILogicComponentSelection("Prefabs/UINotGate", "Sprites/TestSquareTextureRed", "Sprites/TestSquareTexture"),
                this.InstantiateUILogicComponentSelection("Prefabs/UIAndGate", "Sprites/TestSquareTextureRed", "Sprites/TestSquareTexture"),
                this.InstantiateUILogicComponentSelection("Prefabs/UIOrGate", "Sprites/TestSquareTextureRed", "Sprites/TestSquareTexture"),
                this.InstantiateUILogicComponentSelection("Prefabs/UIXorGate", "Sprites/TestSquareTextureRed", "Sprites/TestSquareTexture"),
                this.InstantiateUILogicComponentSelection("Prefabs/UIEdge", "Sprites/TestSquareTextureRed", "Sprites/TestSquareTexture")
            };
            this.CurrentlySelectedComponent = null;
        }

        // Update is called once per frame
        private void Update()
        {
            //
        }

        private UILogicComponentSelection InstantiateUILogicComponentSelection(string logicComponentPrefabPath, string selectedSpritePath, string unselectedSpritePath)
        {
            UILogicComponent prefab = Resources.Load<UILogicComponent>(logicComponentPrefabPath);
            Assert.IsNotNull(prefab);
            Sprite activeSprite = Resources.Load<Sprite>(selectedSpritePath);
            Assert.IsNotNull(activeSprite);
            Sprite inactiveSprite = Resources.Load<Sprite>(unselectedSpritePath);
            Assert.IsNotNull(inactiveSprite);
            return this.InstantiateUILogicComponentSelection(prefab, activeSprite, inactiveSprite);
        }

        private UILogicComponentSelection InstantiateUILogicComponentSelection(UILogicComponent logicComponentPrefab, Sprite activeSprite, Sprite inactiveSprite)
        {
            UILogicComponentSelection newComponentSelection = Instantiate(this.UILogicComponentSelectionPrefab, this.transform);
            Assert.IsNotNull(newComponentSelection);
            newComponentSelection.SetSprites(activeSprite, inactiveSprite);
            newComponentSelection.LogicComponentPrefab = logicComponentPrefab;
            newComponentSelection.gameObject.GetComponent<Image>().sprite = newComponentSelection.InactiveSprite;
            return newComponentSelection;
        }

        public void ToggleSelectedComponent(UILogicComponentSelection componentSelection)
        {
            Assert.IsTrue(this.ComponentSelectionList.Contains(componentSelection));
            this.CurrentlySelectedComponent = (this.CurrentlySelectedComponent == componentSelection) ? null : componentSelection;
            foreach (var component in this.ComponentSelectionList)
            {
                component.gameObject.GetComponent<Image>().sprite = (component == this.CurrentlySelectedComponent) ? component.ActiveSprite : component.InactiveSprite;
            }
        }
    }
}
