using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace __Game.Scripts
{

    public class BulletTower : OffensiveStructure
    {
        private float fireFrequency = 1f;
        private AudioSource audioSource;

        protected override void Awake()
        {
            base.Awake();
            audioSource = GetComponent<AudioSource>();
        }

        protected override void Start()
        {
            base.Start();
            InvokeRepeating(nameof(TryFire), fireFrequency, fireFrequency);
        }

        private void TryFire()
        {
            if (inRange.Count > 0)
            {
                var target = inRange[Random.Range(0, inRange.Count)];
                if (target != null)
                {
                    var firePos = transform.position;
                    firePos.y = 0.1f;
                    var targetPos = target.transform.position;
                    targetPos.y = 0.1f;
                    var vectorToTarget = (targetPos - firePos).normalized;
                    var bullet = Game.Instance.BulletPool.Spawn(firePos, Quaternion.Euler(90, 0, 0));
                    bullet.Direction = vectorToTarget;
                    audioSource.Play();
                }
            }
        }

    }
}