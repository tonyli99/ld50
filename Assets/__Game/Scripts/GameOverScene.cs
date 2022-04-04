using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace __Game.Scripts
{
    public class GameOverScene : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI text;
        [SerializeField] private Button restartButton;
        [SerializeField] private Button quitButton;
        [SerializeField] private Image flashImage;

        private IEnumerator Start()
        {
            restartButton.onClick.AddListener(() =>
            {
                SceneManager.LoadScene(0);
            });
            quitButton.onClick.AddListener(() =>
            {
                Application.Quit();
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#endif
            });
            restartButton.gameObject.SetActive(false);
            quitButton.gameObject.SetActive(false);

            var data = PlayerPrefs.GetInt("Data");
            var highScore = PlayerPrefs.GetInt("HighScore");
            text.text = $"Connection Lost...\nData Received: {data}\nBest Mission: {highScore}";
            text.ForceMeshUpdate();
            text.maxVisibleCharacters = 0;

            flashImage.gameObject.SetActive(true);
            float elapsed = 0;
            while (elapsed < 1)
            {
                flashImage.color = new Color(1, 1, 1, Mathf.Clamp01(1 - elapsed));
                yield return null;
                elapsed += Time.deltaTime;
            }
            flashImage.gameObject.SetActive(false);

            var charDelay = new WaitForSeconds(0.05f);
            while (text.maxVisibleCharacters < text.textInfo.characterCount)
            {
                text.maxVisibleCharacters++;
                yield return charDelay;
            }
            
            restartButton.gameObject.SetActive(true);
            quitButton.gameObject.SetActive(true);
        }

    }
}