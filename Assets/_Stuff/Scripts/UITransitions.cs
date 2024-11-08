using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UITransitions : MonoBehaviour
{
    public CanvasGroup canvas;

    public void FadeIn(int delay)
    {
        StopAllCoroutines();

        StartCoroutine(FadeIn(delay));

        IEnumerator FadeIn(int delay)
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

    public void FadeOut(int delay)
    {
        StopAllCoroutines();

        StartCoroutine(FadeOut(delay));

        IEnumerator FadeOut(int delay)
        {
            canvas.blocksRaycasts = true;
            canvas.interactable = true;

            while (canvas.alpha < 1)
            {
                canvas.alpha += Time.deltaTime / delay;
                yield return null;
            }

            yield return null;
        }
    }
}
