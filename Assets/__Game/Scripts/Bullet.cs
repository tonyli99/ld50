using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace __Game.Scripts
{

    public class Bullet : HealthEntity
    {
        private float speed = 10;
        private float lifetime = 3;
        private int damage = 100;
        
        public Vector3 Direction { get; set; }

        private RaycastHit[] hits = new RaycastHit[10];

        public override void Initialize()
        {
            base.Initialize();
            Invoke(nameof(Despawn), lifetime);
        }

        private void FixedUpdate()
        {
            var newPos = transform.position + Direction * speed * Time.fixedDeltaTime;
            var distance = Vector3.Distance(transform.position, newPos);
            int numHits = Physics.RaycastNonAlloc(transform.position, (newPos - transform.position).normalized, hits,
                distance);
            for (int i = 0; i < numHits; i++)
            {
                var hit = hits[i]; 
                var alien = hit.collider.GetComponent<Alien>();
                if (alien != null) alien.TakeDamage(damage);
                Despawn();
            }
            transform.position = newPos;
        }

        private void Despawn()
        {
            gameObject.SetActive(false);
        }
    }
}
