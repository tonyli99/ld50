using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace __Game.Scripts
{

    public class HealthEntity : MonoBehaviour
    {
        [SerializeField] private float maxHealth = 100;
        [SerializeField] private float health = 100;

        public bool IsPooled { get; set; } = false;
        
        public float MaxHealth
        {
            get { return maxHealth; }
        }
        
        public float Health
        {
            get { return health; }
            set { health = value;}
        }
        
        private HealthBar healthBar;
        
        public HealthBar HealthBar
        {
            get { return healthBar; }
        }

        protected virtual void Awake()
        {
            healthBar = GetComponentInChildren<HealthBar>();
        }

        protected virtual void Start()
        {
            if (healthBar != null) healthBar.Hide();
        }
        
        public virtual void Initialize()
        {
            Health = MaxHealth;
        }
        
        public virtual void TakeDamage(int damageTaken)
        {
            Health -= damageTaken;
            if (Health <= 0)
            {
                if (IsPooled)
                {
                    CancelInvoke();
                    gameObject.SetActive(false);
                }
                else
                {
                    Destroy(gameObject);
                }
            }
            else
            {
                if (healthBar != null) HealthBar.ShowAndFade(Health / MaxHealth);
            }
        }
    }
}
