using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace __Game.Scripts
{
    public class StructureBuildSelector : MonoBehaviour
    {

        [SerializeField] private SpriteRenderer spriteRenderer;

        private StructureRecipeAsset recipe;

        private void Update()
        {
            var pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            spriteRenderer.transform.position = new Vector3(pos.x, 0, pos.z);
            if (recipe != null)
            {
                Vector3 destinationPos = Vector3.zero;
                var canBuild = !EventSystem.current.IsPointerOverGameObject() &&
                               (Game.Instance.Gas >= recipe.gasCost) &&  
                    recipe.structure.CanBuildHere(pos, out destinationPos);
                if (recipe.structure.OverlapsUnits(pos)) canBuild = false;
                spriteRenderer.color = canBuild ? new Color(0, 1, 0, 0.5f) : new Color(1, 0, 0, 0.5f);
                if (canBuild)
                {
                    spriteRenderer.transform.position = destinationPos;
                    if (Input.GetMouseButtonUp(0))
                    {
                        Game.Instance.Build(recipe, destinationPos);
                        SetSelector(null);
                    }
                }
            }
        }

        public void SetSelector(StructureRecipeAsset recipeToSet)
        {
            recipe = recipeToSet;
            spriteRenderer.enabled = recipe != null;
            if (recipe != null)
            {
                spriteRenderer.sprite = recipeToSet.selectorSprite;
            }
        }

    }
}