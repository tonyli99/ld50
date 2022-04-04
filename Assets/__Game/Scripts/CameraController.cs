using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Random = System.Random;

namespace __Game.Scripts
{
    public class CameraController : MonoBehaviour
    {

        [SerializeField] private float speed = 10;
        private Transform myTransform;

        private const float PPU = 64;
        private const float ShakeDuration = 1;
        private const float ShakeAmount = 1f;
        private Vector3 shakeOffset = Vector3.zero;
        private Vector3 cameraPos;
        
        public static CameraController Instance { get; private set; }
        
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
            myTransform = GetComponent<Transform>();
            cameraPos = myTransform.position;
        }

        private void Update()
        {
            var move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

            var mousePos = Input.mousePosition;
            if (!(EventSystem.current.IsPointerOverGameObject() || mousePos.x < 0 || mousePos.y < 0 || mousePos.x > Screen.width || mousePos.y > Screen.height))
            {
                if (mousePos.x < 20) move.x -= 1;
                if (mousePos.x > Screen.width - 20) move.x += 1;
                if (mousePos.y < 20) move.z -= 1;
                if (mousePos.y > Screen.height - 20) move.z += 1;
            }

            var bounds = new Vector3(Game.Instance.Map.Radius - Screen.width / PPU, 0,
                Game.Instance.Map.Radius - Screen.height / PPU);
            var multiplier = (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) ? 4 : 1;
            var newPos = cameraPos + (move * speed * multiplier * Time.deltaTime);
            newPos.x = Mathf.Clamp(newPos.x, -bounds.x, bounds.x);
            newPos.z = Mathf.Clamp(newPos.z, -bounds.z, bounds.z);
            cameraPos = newPos;
            myTransform.position = cameraPos + shakeOffset;
        }

        public void Shake()
        {
            StartCoroutine(ShakeCoroutine());
        }

        private IEnumerator ShakeCoroutine()
        {
            float elapsed = 0;
            while (elapsed < ShakeDuration)
            {
                shakeOffset = new Vector3((UnityEngine.Random.value - 0.5f) * ShakeAmount, 0, (UnityEngine.Random.value - 0.5f) * ShakeAmount);
                yield return null;
                elapsed += Time.deltaTime;
            }
            shakeOffset = Vector3.zero;
        }
    }
}