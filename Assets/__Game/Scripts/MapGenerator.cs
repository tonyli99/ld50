using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace __Game.Scripts
{
    public class MapGenerator : MonoBehaviour
    {

        [SerializeField] private int radius;
        [SerializeField] private Sprite[] groundTiles;
        [SerializeField] private Vent ventPrefab;
        [SerializeField] private Nest nestPrefab;

        private HashSet<int> occupied; 
        
        private void Start()
        {
            Game.Instance.Map = new Map();
            Game.Instance.Map.Radius = radius;
            
            for (var x = -radius; x <= radius; x++)
            {
                for (var z = -radius; z <= radius; z++)
                {
                    var tile = new GameObject($"({x},{z})").AddComponent<SpriteRenderer>();
                    tile.transform.SetParent(transform);
                    tile.transform.position = new Vector3(x, 0, z);
                    tile.transform.rotation = Quaternion.Euler(90, 0, 0);
                    tile.sprite = groundTiles[Random.Range(0, groundTiles.Length)];
                }
            }

            occupied = new HashSet<int>();
            
            Game.Instance.Map.Vents = new List<Vent>();
            AddPrefabs<Vent>(ventPrefab, Game.Instance.Map.Vents, 3, 9, 15);
            AddPrefabs<Vent>(ventPrefab, Game.Instance.Map.Vents, 10, radius / 2, 15);
            AddPrefabs<Vent>(ventPrefab, Game.Instance.Map.Vents, radius / 2 + 1, radius, 15);

            Game.Instance.Map.Nests = new List<Nest>();
            AddPrefabs<Nest>(nestPrefab, Game.Instance.Map.Nests, 10, radius / 2, 5);
            AddPrefabs<Nest>(nestPrefab, Game.Instance.Map.Nests, radius / 2 + 1, radius, 5);
        }

        private void AddPrefabs<T>(T prefab, List<T> list, int min, int max, int count) where T : MonoBehaviour 
        {
            for (var i = 0; i < count; i++)
            {
                int safeguard = 0;
                int x, z, pos;
                do
                {
                    x = Random.Range(min, max);
                    if (Random.value < 0.5f) x *= -1;
                    z = Random.Range(min, max);
                    if (Random.value < 0.5f) z *= -1;
                    pos = x * 1000 + z;
                } while (!occupied.Contains(pos) && ++safeguard < 100);

                if (!occupied.Contains(pos))
                {
                    occupied.Add(pos);
                    var instance = Instantiate(prefab, new Vector3(x, 0, z), Quaternion.Euler(90, 0, 0));
                    instance.transform.SetParent(transform);
                    list.Add(instance);
                }
            }

        }
    }
}
