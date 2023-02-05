using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/* 
 TRANSITIONS:
INSTANT - instant
FADE - opacity fade

while character or background is "transitioning" in, disable ability to move on.
 */
public class ScreenState : MonoBehaviour
{
    public static ScreenState instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Load(GameObject obj, string transition, float duration)
    {
        if (transition == "FADE")
        {
            StartCoroutine(FadeIn(obj.GetComponent<Image>(), transition, duration));
        }
    }

    public void Offload(GameObject obj, string transition, float duration)
    {
        if (transition == "FADE")
        {
            StartCoroutine(FadeOut(obj.GetComponent<Image>(), transition, duration));
        } else
        {
            Destroy(obj);
        }
    }

    public IEnumerator FadeIn(Image img, string transition, float duration)
    {
        float between = duration / 100;
        img.color = new Color(1, 1, 1, 0);
        
        for (float i = 0; i <= 100; i++)
        {
            
            img.color = new Color(1, 1, 1, i / 100f);

            yield return new WaitForSeconds(between);
        }

        img.color = new Color(1, 1, 1, 1);
    }

    public IEnumerator FadeOut(Image img, string transition, float duration)
    {
        float between = duration / 100;

        for (float i = 100; i >= 0; i--)
        {
            img.color = new Color(1, 1, 1, i / 100f);

            yield return new WaitForSeconds(between);
        }

        Destroy(img.gameObject);
    }
}
