using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace __Game.Scripts
{

    public class Barrier : OffensiveStructure
    {
        [SerializeField] private int damage = 0;
        [SerializeField] private float damageFrequency = 0.1f;


        protected override void Start()
        {
            base.Start();
            if (damage > 0)
            {
                InvokeRepeating(nameof(DamageInRange), damageFrequency, damageFrequency);
            }
        }
        

        private void DamageInRange()
        {
            if (inRange.Count > 0)
            {
                foreach (var alien in inRange)
                {
                    if (alien != null)
                    {
                        alien.TakeDamage(damage);
                    }
                }
                inRange.RemoveAll(alien => alien.Health <= 0);
            }
        }
    }
}