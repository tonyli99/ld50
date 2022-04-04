using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace __Game.Scripts
{

    public class LaserTower : OffensiveStructure
    {
        [SerializeField] private LineRenderer line;

        private int damage = 50;
        private float fireFrequency = 1f;
        private float lineVisibleDuration = 0.2f;
        private float laserDistance = 20f;

        private RaycastHit[] hits;

        protected override void Start()
        {
            base.Start();
            line.gameObject.SetActive(false);
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
                    var angleToTarget = Vector3.Angle(Vector3.forward, vectorToTarget);
                    line.transform.rotation = Quaternion.Euler(angleToTarget, 90, 0);
                    line.gameObject.SetActive(true);
                    Invoke(nameof(HideLine), lineVisibleDuration);
                    //Debug.DrawRay(firePos, vectorToTarget * laserDistance, Color.magenta, 1); 
                    int numHits = Physics.RaycastNonAlloc(firePos, vectorToTarget, hits, laserDistance);
                    for (int i = 0; i < numHits; i++)
                    {
                        var alien = hits[i].collider.GetComponent<Alien>();
                        if (alien != null) alien.TakeDamage(damage);
                    }
                }
            }
        }

        private void HideLine()
        {
            line.gameObject.SetActive(false);
        }
    }
}
