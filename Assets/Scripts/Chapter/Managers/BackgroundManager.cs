using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundManager : MonoBehaviour
{
    public static BackgroundManager instance;

    [SerializeField]
    private Transform screenBackground;
    [SerializeField]
    private GameObject ImagePrefab;
    private Sprite[] backgrounds;

    [HideInInspector]
    public GameObject currentBackground;
    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        } else
        {
            Destroy(gameObject);
        }

        backgrounds = Resources.LoadAll<Sprite>("bg");
    }

    public void OnResize()
    {
        if (currentBackground != null)
            Resize(currentBackground);
    }

    public void SetBackground(string name, string transition, float duration)
    {
        // offload one if it exists
        HideBackground(transition, duration);

        for (int i = 0; i < backgrounds.Length; i++)
        {
            if (backgrounds[i].name == name)
            {

                GameObject img = Instantiate(ImagePrefab);
                img.transform.SetParent(screenBackground);
                img.name = name;
                img.GetComponent<Image>().sprite = backgrounds[i];
                img.GetComponent<Image>().preserveAspect = true;

                Resize(img);

                currentBackground = img;
                ScreenState.instance.Load(img, transition, duration);

                return;
            }
        }

        Debug.LogError("Invalid Background: " + name);
    }

    public void HideBackground(string transition, float duration)
    {
        if (currentBackground != null)
        {
            ScreenState.instance.Offload(currentBackground, transition, duration);
            currentBackground = null;
        }
    }

    public void Resize(GameObject img)
    {
        /* MAKE SURE THAT width and height are completely filled at least */
        Sprite bg = img.GetComponent<Image>().sprite;
        RectTransform rect = img.GetComponent<RectTransform>();
        RectTransform screen = screenBackground.GetComponent<RectTransform>();

        float w = bg.bounds.size.x * 100;
        float h = bg.bounds.size.y * 100;

        float wscale = screen.rect.width / w;
        float hscale = screen.rect.height / h;

        if (wscale * h >= screen.rect.height)
        {
            w *= wscale;
            h *= wscale;
        } else
        {
            w *= hscale;
            h *= hscale;
        }

        rect.sizeDelta = new Vector2(w, h);
        rect.anchoredPosition = new Vector2(0, 0);
    }
}
