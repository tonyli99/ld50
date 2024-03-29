using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.AI;

namespace __Game.Scripts
{

    public class Alien : HealthEntity
    {

        [SerializeField] private bool isExploder;
        [SerializeField] private AudioClip biteSound;
        [SerializeField] private AudioClip dieSound;
        
        private int damage = 2;
        private float damageRate = 0.5f;
        
        private NavMeshAgent navMeshAgent;
        private Structure target = null;
        private float timeNextDamage;
        private Base playerBase;
        private AudioSource audioSource;

        protected override void Awake()
        {
            base.Awake();
            navMeshAgent = GetComponent<NavMeshAgent>();
            playerBase = FindObjectOfType<Base>();
            audioSource = GetComponent<AudioSource>();
        }

        protected override void Start()
        {
            base.Start();
            audioSource.clip = biteSound;
            GotoBase();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                navMeshAgent.SetDestination(other.transform.position);
                target = other.GetComponent<Structure>();
            }
        }

        private void Update()
        {
            if (target != null)
            {
                if (Time.time >= timeNextDamage &&
                    Vector3.Distance(transform.position, target.transform.position) < 1)
                {
                    timeNextDamage = Time.time + damageRate;
                    navMeshAgent.isStopped = true;
                    target.TakeDamage(damage);
                    audioSource.Play();
                }
                if (target.Health <= 0)
                {
                    target = null;
                    GotoBase();
                }
            }
            else if (navMeshAgent.isStopped)
            {
                GotoBase();
            }
        }

        private void GotoBase()
        {
            if (playerBase == null || playerBase.Health <= 0)
            {
                enabled = false;
            }
            else
            {
                navMeshAgent.SetDestination(playerBase.transform.position);
                navMeshAgent.isStopped = false;
            }
        }

        public override void TakeDamage(int damageTaken)
        {
            base.TakeDamage(damageTaken);
            if (Health <= 0)
            {
                if (isExploder)
                {
                    Game.Instance.ExplosionPool.Spawn(transform.position, Quaternion.identity);
                }
                else
                {
                    AudioSource.PlayClipAtPoint(dieSound, transform.position);
                }
            }
        }
    }
}