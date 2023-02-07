using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterManager : MonoBehaviour
{
    public static CharacterManager instance;

    [SerializeField]
    private float yOffset = 50;
    [SerializeField]
    private float yOffsetClose = 300;

    [SerializeField]
    private GameObject screenCharacters;
    private RectTransform screen;
    [SerializeField]
    private GameObject ImagePrefab;

    private Sprite[] characterSprites;

    private List<string> characters = new List<string>();
    private List<float> characterPositions = new List<float>(); // 1-4 inclusive floats.
    private List<GameObject> loadedCharacters = new List<GameObject>();

    private string closeCharacter;
    private GameObject closeObject;

    private float prevWidth;
    private float prevHeight;

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

        characterSprites = Resources.LoadAll<Sprite>("characters");
        screen = screenCharacters.GetComponent<RectTransform>();
        prevWidth = screen.rect.width;
    }

    private void Update()
    {
        if (screen.rect.width != prevWidth || screen.rect.height != prevHeight)
        {
            prevWidth = screen.rect.width;
            prevHeight = screen.rect.height;
            for (int i = 0; i < loadedCharacters.Count; ++i)
                if (loadedCharacters[i] != null)
                    Resize(loadedCharacters[i], characterPositions[i]);
            if (closeObject != null) ResizeClose(closeObject);
            BackgroundManager.instance.OnResize(); // scrappy workaround to be more efficient
        }
    }

    public void ShowCharacter(float slot, string name, string transition, float duration)
    {
        if (slot < 1 || slot > 4)
        {
            Debug.LogError("Invalid Slot: " + slot);
            return;
        }

        for (int i = 0; i < characters.Count; ++i)
        {
            if (slot == characterPositions[i])
            {
                HideCharacter(characters[i].Split(".")[0], transition, duration);
                break;
            }
        }

        for (int i = 0; i < characterSprites.Length; i++)
        {
            if (characterSprites[i].name == name)
            {
                GameObject img = Instantiate(ImagePrefab);
                img.transform.SetParent(screenCharacters.transform);
                img.name = name;
                img.GetComponent<Image>().sprite = characterSprites[i];
                img.GetComponent<Image>().preserveAspect = true;

                Resize(img, slot);
                ScreenState.instance.Load(img, transition, duration);

                loadedCharacters.Add(img);
                characterPositions.Add(slot);
                characters.Add(name);

                return;
            }
        }

        Debug.LogError("Invalid CharacterSprite: " + name);
    }

    /* HIDE THIS WHEN CLOSE CHARACTER IS DONE SPEAKING. 
     * This is triggered by DialogueSystem rather than SHOW command */
    public void ShowCloseCharacter(string name)
    {
        if (closeCharacter != null && closeCharacter == name) return;
        if (closeCharacter != null && closeCharacter != name)
        {
            HideCloseCharacter();
        }

        /* SET ALL OTHER CHARACTERS ON SCREEN TO NOT ACTIVE */
        for (int i = 0; i < loadedCharacters.Count; i++)
        {
            if (loadedCharacters[i] == null) continue;
            loadedCharacters[i].SetActive(false);
        }

        for (int i = 0; i < characterSprites.Length; i++)
        {
            if (characterSprites[i].name == name)
            {
                GameObject img = Instantiate(ImagePrefab);
                img.transform.SetParent(screenCharacters.transform);
                img.name = name;
                img.GetComponent<Image>().sprite = characterSprites[i];
                img.GetComponent<Image>().preserveAspect = true;

                closeCharacter = name;
                closeObject = img;

                ResizeClose(img);
                return;
            }
        }

        Debug.LogError("Invalid CharacterSprite: " + name);
    }

    public void HideCharacter(string name, string transition, float duration)
    {
        int ind = -1;
        for (int i = 0; i < characters.Count; i++)
        {
            if (characters[i].Split(".")[0] == name)
            {
                ind = i;
                break;
            }
        }

        if (ind == -1)
        {
            Debug.LogError("Invalid ScreenCharacter: " + name);
            return;
        }

        /* TRANSITION CHARACTER OUT + HIDE */
        GameObject loaded = loadedCharacters[ind];
        ScreenState.instance.Offload(loaded, transition, duration);

        characters.RemoveAt(ind);
        characterPositions.RemoveAt(ind);
        loadedCharacters.RemoveAt(ind);
    }

    public void HideCloseCharacter()
    {
        if (closeCharacter != null)
        {
            Destroy(closeObject);
            closeCharacter = null;
            closeObject = null;

            /* ENABLE ALL OTHER CHARACTERS */
            for (int i = 0; i < loadedCharacters.Count; i++)
            {
                if (loadedCharacters[i] == null) continue;
                loadedCharacters[i].SetActive(true);
            }
        }
    }

    public void Resize(GameObject img, float slot)
    {
        RectTransform rect = img.GetComponent<RectTransform>();

        float w = (screen.rect.width + 80) / 4;
        float h = w * (1080 / 500);
        rect.sizeDelta = new Vector2(w, h);

        if (slot == 1)
        {
            rect.anchoredPosition = new Vector2(w / 2, (h / 2) - yOffset);
        }
        else
        {
            rect.anchoredPosition = new Vector2((w / 2) - 20 + (slot - 1) * (screen.rect.width / 4), (h / 2) - yOffset);
        }
    } 

    public void ResizeClose(GameObject img)
    {
        RectTransform rect = img.GetComponent<RectTransform>();

        float w = (screen.rect.width + 80) / (2.5f);
        float h = w * (1080 / 500);
        rect.sizeDelta = new Vector2(w, h);
        rect.anchoredPosition = new Vector2(screen.rect.width/2, (h / 2) - yOffsetClose);
    }

    public Sprite GetSprite(string name)
    {
        for (int i = 0; i < characterSprites.Length; i++)
        {
            if (characterSprites[i].name == name)
            {
                return characterSprites[i]; 
            }
        }

        return null;
    }
}
