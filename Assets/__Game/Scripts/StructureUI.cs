using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Pipes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace __Game.Scripts
{

    public class StructureUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI dataTransferredText;
        [SerializeField] private TextMeshProUGUI gasAmountText;
        [SerializeField] private TextMeshProUGUI messageText;
        [SerializeField] private GameObject structurePanel;
        [SerializeField] private TextMeshProUGUI structureNameText;
        [SerializeField] private Transform buttonContainer;
        [SerializeField] private Button buttonTemplate;
        
        private Structure structure;
        private List<Button> instantiatedButtons = new List<Button>();

        private const float ShowMessageDuration = 3;

        private void Awake()
        {
            messageText.gameObject.SetActive(false);
        }

        public void SetDataAmount(int amount)
        {
            dataTransferredText.text = amount.ToString();
        }

        public void SetGasAmount(int gas)
        {
            gasAmountText.text = gas.ToString();
        }
        
        public void InspectStructure(Structure structureToInspect)
        {
            structure = structureToInspect;
            var isInspectable = structureToInspect != null && structureToInspect.Recipes.Length > 0;
            structurePanel.SetActive(isInspectable);
            if (!isInspectable) return;
            instantiatedButtons.ForEach(x => Destroy(x.gameObject));
            instantiatedButtons.Clear();
            if (structure != null)
            {
                structureNameText.text = structure.name;
                buttonTemplate.gameObject.SetActive(true);
                for (int i = 0; i < structure.Recipes.Length; i++)
                {
                    var recipe = structure.Recipes[i];
                    var button = Instantiate(buttonTemplate, buttonContainer);
                    instantiatedButtons.Add(button);
                    button.GetComponent<Image>().sprite = recipe.buttonSprite;
                    button.GetComponentInChildren<TextMeshProUGUI>().text = $"{recipe.name}\n{recipe.gasCost}";
                    button.onClick.AddListener(() =>
                    {
                        TryBuildStructure(recipe);
                    });
                }
                buttonTemplate.gameObject.SetActive(false);
            }
        }

        private void TryBuildStructure(StructureRecipeAsset recipe)
        {
            if (recipe == null) return;
            if (Game.Instance.Gas < recipe.gasCost)
            {
                ShowMessage("Not enough energy!");
            }
            else
            {
                Game.Instance.StructureBuildSelector.SetSelector(recipe);
            }
        }

        public void ShowMessage(string message)
        {
            messageText.gameObject.SetActive(true);
            messageText.text = message;
            CancelInvoke(nameof(HideMessage));
            Invoke(nameof(HideMessage), ShowMessageDuration);
        }

        private void HideMessage()
        {
            messageText.gameObject.SetActive(false);
        }
        
    }
}