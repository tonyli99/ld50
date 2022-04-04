using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace __Game.Scripts
{

    public class MissionBrief : MonoBehaviour
    {
        [SerializeField] private Button playButton;
        [SerializeField] private Button quitButton;

        private void Start()
        {
            Time.timeScale = 0;
            playButton.onClick.AddListener(() =>
            {
                Time.timeScale = 1;
                gameObject.SetActive(false);
            });
            quitButton.onClick.AddListener(() =>
            {
                Application.Quit();
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#endif
            });
        }
    }
}
