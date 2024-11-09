using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class UITransitions : MonoBehaviour
{
    public static UITransitions Instance;
    public CanvasGroup canvas;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            FadeIn(.8f);
        }
        else
            Destroy(gameObject);
    }

    public void FadeIn(float delay)
    {
        StopAllCoroutines();

        StartCoroutine(FadeIn(delay));

        IEnumerator FadeIn(float delay)
        {
            while (canvas.alpha > 0)
            {
                canvas.alpha -= Time.deltaTime / delay;
                yield return null;
            }

            canvas.alpha = 0;
            canvas.interactable = false;
            canvas.blocksRaycasts = false;

            yield return null;
        }

    }

    public void FadeOut(float delay, string sceneToTransition)
    {
        StopAllCoroutines();

        StartCoroutine(FadeOut(delay));

        IEnumerator FadeOut(float delay)
        {
            canvas.blocksRaycasts = true;
            canvas.interactable = true;

            while (canvas.alpha < 1)
            {
                canvas.alpha += Time.deltaTime / delay;
                yield return null;
            }

            yield return null;

            if (!string.IsNullOrWhiteSpace(sceneToTransition))
                UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneToTransition).completed += OnLoadSceneComplete;
        }
    }

    private void OnLoadSceneComplete(AsyncOperation operation)
    {
        FadeIn(0.4f);
    }
}
