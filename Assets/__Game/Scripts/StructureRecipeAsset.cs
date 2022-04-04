using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace __Game.Scripts
{
    [CreateAssetMenu]
    public class StructureRecipeAsset : ScriptableObject
    {

        public Sprite buttonSprite;
        public Sprite selectorSprite;
        public Structure structure;
        public int gasCost;

    }
}