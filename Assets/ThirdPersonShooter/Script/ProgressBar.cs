using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
#if UNITY_EDITOR
#endif

namespace ThirdPersonShooter.Script
{
    public class ProgressBar : MonoBehaviour
    {
        [SerializeField] private Image ProgressImage;
        [SerializeField] private float DefaultSpeed = 1f;
        [SerializeField] private Gradient ColorGradient;
        [SerializeField] private UnityEvent<float> OnProgress;
        [SerializeField] private UnityEvent OnCompleted;

        private Coroutine AnimationCoroutine;

        private void Start()
        {
            if (ProgressImage.type != Image.Type.Filled)
            {
                Debug.LogError(
                    $"{name}'s ProgressImage is not of type \"Filled\" so it cannot be used as a progress bar. Disabling this Progress Bar.");
                enabled = false;
#if UNITY_EDITOR
                EditorGUIUtility.PingObject(this.gameObject);
#endif
            }
        }

        public void SetProgress(float progress)
        {
            SetProgress(progress, DefaultSpeed);
        }

        public void SetProgress(float progress, float speed)
        {
            if (progress < 0 || progress > 1)
            {
                Debug.LogWarning($"Invalid progress passed, expected value is between 0 and 1, got {progress}. Clamping.");
                progress = Mathf.Clamp01(progress);
            }

            if (progress == ProgressImage.fillAmount) return;

            if (AnimationCoroutine != null)
                StopCoroutine(AnimationCoroutine);

            AnimationCoroutine = StartCoroutine(AnimateProgress(progress, speed));
        }

        private IEnumerator AnimateProgress(float progress, float speed)
        {
            float time = 0;
            float initialProgress = ProgressImage.fillAmount;

            while (time < 1)
            {
                ProgressImage.fillAmount = Mathf.Lerp(initialProgress, progress, time);
                time += Time.deltaTime * speed;

                ProgressImage.color = ColorGradient.Evaluate(1 - ProgressImage.fillAmount);

                OnProgress?.Invoke(ProgressImage.fillAmount);
                yield return null;
            }

            ProgressImage.fillAmount = progress;
            ProgressImage.color = ColorGradient.Evaluate(1 - ProgressImage.fillAmount);

            OnProgress?.Invoke(progress);
            OnCompleted?.Invoke();
        }
    }
}