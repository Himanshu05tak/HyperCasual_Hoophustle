using UnityEngine;
using System.Collections;

namespace UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public class UIFadeController : MonoBehaviour
    {
        private const float FADE_DURATION = 1f;
    
        private Coroutine _fadeCoroutine;
        private CanvasGroup _canvasGroup;

        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        public void FadeIn()
        {
            StartFade(1f, true);
        }

        public void FadeOut()
        {
            StartFade(0f, false);
        }

        private void StartFade(float targetAlpha, bool interactable)
        {
            if (_fadeCoroutine != null)
                StopCoroutine(_fadeCoroutine);

            _fadeCoroutine = StartCoroutine(
                FadeCanvasGroup(targetAlpha, interactable)
            );
        }
        private IEnumerator FadeCanvasGroup(float targetAlpha, bool interactable)
        {
            var startAlpha = _canvasGroup.alpha;
            var time = 0f;

            while (time < FADE_DURATION)
            {
                time += Time.deltaTime;

                _canvasGroup.alpha = Mathf.Lerp(
                    startAlpha,
                    targetAlpha,
                    time / FADE_DURATION
                );

                yield return null;
            }

            _canvasGroup.alpha = targetAlpha;

            _canvasGroup.interactable = interactable;
            _canvasGroup.blocksRaycasts = interactable;

            //_fadeCoroutine = null;
        }
    }
}
