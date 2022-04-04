using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace __Game.Scripts
{

    public class Vent : MonoBehaviour
    {
        [SerializeField] private int gas = 100;

        public int Gas
        {
            get { return gas; }
            set { gas = value; }
        }

        public bool IsClaimed { get; set; } = false;
    }
}