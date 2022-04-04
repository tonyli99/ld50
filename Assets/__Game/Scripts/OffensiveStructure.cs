using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace __Game.Scripts
{

    public class OffensiveStructure : Structure
    {
        protected List<Alien> inRange = new List<Alien>();

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Enemy"))
            {
                var alien = other.GetComponent<Alien>();
                if (alien != null && !inRange.Contains(alien))
                {
                    inRange.Add(alien);
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Enemy"))
            {
                inRange.Remove(other.GetComponent<Alien>());
            }
        }
    }
}