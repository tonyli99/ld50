using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace __Game.Scripts
{

    public class Game : MonoBehaviour
    {
        [SerializeField] private Alien alienPrefab;
        [SerializeField] private Alien alienWallbreakerPrefab;
        [SerializeField] private Explosion explosionPrefab;
        [SerializeField] private Bullet bulletPrefab;
        [SerializeField] private Image flashImage;
        [SerializeField] private AudioClip buildSound;
        
        private int startingGas = 200;
        
        public Map Map { get; set; }
        public StructureUI StructureUI { get; private set; }
        public StructureBuildSelector StructureBuildSelector { get; private set; }
        
        public Pool<Alien> AlienPool { get; private set; }
        public Pool<Alien> AlienWallbreakerPool { get; private set; }
        public Pool<Explosion> ExplosionPool { get; private set; }
        public Pool<Bullet> BulletPool { get; private set; }
        
        public AudioSource AudioSource { get; private set; }

        private int dataTransferred;
        private int gas;

        public int Gas
        {
            get { return gas; }
            set
            {
                gas = value;
                StructureUI.SetGasAmount(value);
            }
        }

        public static Game Instance { get; private set; }
        
#if UNITY_EDITOR
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void Init()
        {
            Instance = null;
        }
#endif

        private void Awake()
        {
            Instance = this;
            StructureUI = FindObjectOfType<StructureUI>();
            StructureBuildSelector = FindObjectOfType<StructureBuildSelector>();
            AlienPool = new Pool<Alien>(alienPrefab);
            AlienWallbreakerPool = new Pool<Alien>(alienWallbreakerPrefab);
            ExplosionPool = new Pool<Explosion>(explosionPrefab);
            BulletPool = new Pool<Bullet>(bulletPrefab);
            AudioSource = GetComponent<AudioSource>();
            flashImage.gameObject.SetActive(false);
        }

        private void Start()
        {
            dataTransferred = 0;
            Gas = startingGas;
            StructureUI.InspectStructure(null);
            StructureBuildSelector.SetSelector(null);
            InvokeRepeating(nameof(TransferData), 1, 1);
        }

        private void Update()
        {
            if (Input.GetMouseButtonUp(1))
            {
                CancelSelection();
            }
        }

        private void TransferData()
        {
            dataTransferred++;
            StructureUI.SetDataAmount(dataTransferred);
        }

        public void CancelSelection()
        {
            StructureUI.InspectStructure(null);
            StructureBuildSelector.SetSelector(null);
        }

        public void Build(StructureRecipeAsset recipe, Vector3 destinationPos)
        {
            Gas -= recipe.gasCost;
            var instance = Instantiate(recipe.structure, destinationPos, Quaternion.Euler(90, 0, 0));
            instance.name = recipe.name;
            AudioSource.PlayClipAtPoint(buildSound, transform.position, 0.1f);
        }

        public void GameOver()
        {
            CancelInvoke();
            PlayerPrefs.SetInt("Data", dataTransferred);
            if (dataTransferred > PlayerPrefs.GetInt("HighScore"))
            {
                PlayerPrefs.SetInt("HighScore", dataTransferred);
            }
            StartCoroutine(nameof(FlashAndLoadGameOverScene));
        }

        private IEnumerator FlashAndLoadGameOverScene()
        {
            flashImage.gameObject.SetActive(true);
            float elapsed = 0;
            while (elapsed < 1)
            {
                flashImage.color = new Color(1, 1, 1, Mathf.Clamp01(elapsed));                
                yield return null;
                elapsed += Time.deltaTime;
            }
            flashImage.color = new Color(1, 1, 1, 1);
            SceneManager.LoadScene(1);
        }
    }
}