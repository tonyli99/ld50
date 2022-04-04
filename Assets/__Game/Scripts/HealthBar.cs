using System.Collections;
using UnityEngine;

namespace __Game.Scripts
{

    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer fillRenderer;

        private const float ShowDuration = 0.5f;
        private const float FadeDuration = 0.5f;
        
        private SpriteRenderer spriteRenderer;
        private Color originalSpriteRendererColor;
        private Color fadedSpriteRendererColor;
        private Color originalFillColor;
        private Color fadedFillColor;
        private Coroutine fadeCoroutine;
        private WaitForSeconds waitForFade = new WaitForSeconds(ShowDuration);

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void Start()
        {
            originalSpriteRendererColor = new Color(0, 0, 0, 0.75f);
            fadedSpriteRendererColor = new Color(0, 0, 0, 0);
            originalFillColor = new Color(1, 0, 0, 1);
            fadedFillColor = new Color(1, 0, 0, 0);
        }

        public void Show(float percent)
        {
            SetFill(percent);
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void ShowAndFade(float percent)
        {
            Show(percent);
            if (fadeCoroutine != null)
            {
                StopCoroutine(fadeCoroutine); 
            }
            fadeCoroutine = StartCoroutine(Fade());
        }

        private IEnumerator Fade()
        {
            yield return waitForFade;
            float elapsed = 0;
            while (elapsed < FadeDuration)
            {
                var t = Mathf.Clamp01(elapsed / FadeDuration);
                fillRenderer.color = Color.Lerp(originalFillColor, fadedFillColor, t);
                spriteRenderer.color = Color.Lerp(originalSpriteRendererColor, fadedSpriteRendererColor, t);
                yield return null;
                elapsed += Time.deltaTime;
            }
            Hide();
            fadeCoroutine = null;
        }
        
        private void SetFill(float percent)
        {
            fillRenderer.transform.localScale = new Vector3(percent, 1, 1);
            fillRenderer.color = originalFillColor;
            spriteRenderer.color = originalSpriteRendererColor;
        }
    }
}