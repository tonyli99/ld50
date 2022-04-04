using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace __Game.Scripts
{

    public class Nest : MonoBehaviour
    {

        private float initialSpawnDelay = 10;
        private float spawnFrequency = 10;
        private float emergeFrequency = 0.1f;
        private float timeToEmerge;
        private int numToSpawn;
        private int broodSize = 1;
        private int numSpawned = 0;

        private void Start()
        {
            InvokeRepeating(nameof(Spawn), spawnFrequency, initialSpawnDelay);
        }

        private void Update()
        {
            if (numToSpawn > 0 && Time.time > timeToEmerge)
            {
                timeToEmerge = Time.time + emergeFrequency;
                numToSpawn--;
                numSpawned++;
                if (numSpawned % 10 == 0)
                {
                    Game.Instance.AlienWallbreakerPool.Spawn(transform.position, Quaternion.identity);   
                }
                else
                {
                    Game.Instance.AlienPool.Spawn(transform.position, Quaternion.identity);
                }
            }
        }

        private void Spawn()
        {
            numToSpawn += broodSize;
            broodSize++;
        }
    }
}