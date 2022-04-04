using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace __Game.Scripts
{

    public class Base : Structure
    {
        public override void TakeDamage(int damage)
        {
            base.TakeDamage(damage);
            if (Health <= 0)
            {
                Game.Instance.GameOver();
            }
        }
    }
}