using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace __Game.Scripts
{

    public class Explosion : HealthEntity
    {
        [SerializeField] private float duration = 1;
        [SerializeField] private AudioClip sound;

        private Collider[] hits = new Collider[16];
        private const int ExplosionDamage = 1000;

        public override void Initialize()
        {
            base.Initialize();
            CameraController.Instance.Shake();
            AudioSource.PlayClipAtPoint(sound, transform.position);
            GetComponent<ParticleSystem>().Play();

            var numHits = Physics.OverlapSphereNonAlloc(transform.position, 0.5f, hits);
            for (int i = 0; i < numHits; i++)
            {
                if (hits[i].CompareTag("Player"))
                {
                    var structure = hits[i].GetComponent<Structure>();
                    if (structure != null && !(structure is Base))
                    {
                        structure.TakeDamage(ExplosionDamage);
                    }
                }
            }
            Invoke(nameof(Deactivate), duration);
        }
        
        private void Deactivate()
        {
            gameObject.SetActive(false);
        }
    }
}
