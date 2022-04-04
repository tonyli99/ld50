using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace __Game.Scripts
{

    public class Structure : HealthEntity
    {
        
        [SerializeField] private ParticleSystem fireParticleSystem;

        [SerializeField] private StructureRecipeAsset[] recipes;

        private Collider[] overlapResults = new Collider[5];
        
        public StructureRecipeAsset[] Recipes
        {
            get { return recipes; }
        }
        
        private void OnMouseOver()
        {
            UpdateHealthBar();
            if (Input.GetMouseButtonUp((0)))
            {
                Game.Instance.StructureUI.InspectStructure(this);
            }
        }

        private void OnMouseExit()
        {
            HealthBar.ShowAndFade(Health / MaxHealth);
        }

        public override void Initialize()
        {
            base.Initialize();
            if (fireParticleSystem != null) fireParticleSystem.Stop();
        }

        protected void UpdateHealthBar()
        {
            HealthBar.Show(Health / MaxHealth);
        }

        public virtual bool CanBuildHere(Vector3 pos, out Vector3 destinationPos)
        {
            var unitPos = new Vector3(Mathf.RoundToInt(pos.x), 0, Mathf.RoundToInt(pos.z));
            if (!OverlapsUnits(unitPos))
            {
                destinationPos = unitPos;
                return true;
            }
            else
            {
                destinationPos = Vector3.zero;
                return false;
            }
        }
        
        public bool OverlapsUnits(Vector3 pos)
        {
            Debug.DrawLine(pos + new Vector3(1, 0, 1), pos + new Vector3(-1, 0, -1), Color.red);
            RaycastHit hit;
            if (Physics.Raycast(pos + Vector3.up, Vector3.down, out hit))
            {
                if (hit.collider.CompareTag("Player") || hit.collider.CompareTag("Enemy"))
                {
                    return true;
                }
            }
            return false;
        }

        public override void TakeDamage(int damage)
        {
            base.TakeDamage(damage);
            if (0 < Health && Health < (0.25f * MaxHealth) && !fireParticleSystem.isPlaying)
            {
                fireParticleSystem.Play();
                fireParticleSystem.GetComponent<AudioSource>().Play();
            }
        }

    }
}