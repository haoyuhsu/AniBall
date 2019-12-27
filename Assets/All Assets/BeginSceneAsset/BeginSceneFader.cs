using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BeginSceneFader : MonoBehaviour
{
    public Image img;
    public AnimationCurve curve;

    void Start ()
    {
        StartCoroutine(FadeIn());
    }

    void FadeTo (string scene)
    {
        StartCoroutine(FadeOut(scene));
    }

    IEnumerator FadeIn ()
    {
        float t = 1f;

        while (t > 0f)
        {
            t -= Time.unscaledDeltaTime;
            float a = curve.Evaluate(t);
            img.color = new Color (0f, 0f, 0f, t);
            yield return 0;
        }

        yield return new WaitForSeconds(5.0f);

        FadeTo("Menu");
    }

    IEnumerator FadeOut (string scene)
    {
        float t = 0f;

        while (t < 1f)
        {
            t += Time.unscaledDeltaTime;
            float a = curve.Evaluate(t);
            img.color = new Color (0f, 0f, 0f, t);
            yield return 0;
        }

        yield return new WaitForSeconds(2.5f);

        SceneManager.LoadScene(scene);
    }
}
