using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueSystem : MonoBehaviour
{
    public static DialogueSystem instance;

    [SerializeField]
    private GameObject dialogueBorder;
    [SerializeField]
    private GameObject dialogueBox;
    [SerializeField]
    private GameObject nameBorder;
    [SerializeField]
    private GameObject nameBox;
    [SerializeField]
    private GameObject headshotBox;

    private TextMeshProUGUI dialogue;
    private TextMeshProUGUI nameText;
    private Image headshot;

    // If DialogueSystem is in the middle of "writing" current dialogue line
    private bool writing = false;
    private string[] target;
    int progress = 0;

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

        dialogue = dialogueBox.GetComponent<TextMeshProUGUI>();
        nameText = nameBox.GetComponent<TextMeshProUGUI>();
        headshot = headshotBox.GetComponent<Image>();
        headshot.preserveAspect = true;
    }

    private void Start()
    {
        dialogue.text = "...";
    }

    private IEnumerator Write(float between)
    {
        writing = true;

        while(progress < target.Length)
        {
            dialogue.text += target[progress] + " ";
            progress++;
            yield return new WaitForSeconds(between);
        }

        dialogue.text = string.Join(" ", target);
        if (writing)
        {
            FinishWriting(); // call finish writing if hasn't been called yet
            writing = false;
        }
    }

    public void Talk(string message)
    {
        string[] line = message.Split(" ");
        string state = line[0];

        SetState(state);

        dialogueBorder.SetActive(true);
        dialogue.text = "";

        target = new string[line.Length - 1];
        for (int i = 1; i < line.Length; i++)
        {
            target[i - 1] = line[i];
        }

        progress = 0;

        if (ChapterReader.instance.Voice() == null)
        {
            StartCoroutine(Write(0.10f));
        } else
        {
            // Finish writing by end of voice
            AudioClip v = ChapterReader.instance.Voice().GetComponent<AudioSource>().clip;
            StartCoroutine(Write(((float) v.length / (float) target.Length) + 0.01f)); 
        }
        
    }

    public bool IsWriting()
    {
        return writing;
    }

    public void Disable()
    {
        dialogueBorder.SetActive(false);
    }

    public void FinishWriting()
    {
        if (writing) {
            progress = target.Length;
            writing = false;
            ChapterReader.instance.FinishReadingLine();
        }
    }

    // set screen states + dialogue box states
    private void SetState(string state)
    {
        // CHARACTER.STATE.CLOSE?
        string[] args = state.Split(".");
        
        string chara = args[0];
        string emotion = "NEUTRAL";
        
        bool close = false;

        if (chara == "NARRATOR")
        {
            nameBorder.SetActive(false);
        } else
        {
            string formattedName = "";
            foreach (string p in chara.Split(' '))
            {
                formattedName += p.Substring(0, 1).ToUpper() + chara.Substring(1).ToLower() + " ";
            }
            formattedName = formattedName.Trim();

            nameText.text = formattedName;
            nameBorder.SetActive(true);
        }

        if (args.Length >= 2)
        {
            emotion = args[1];
            if (args.Length >= 3)
            {
                close = args[2] == "CLOSE";
            }
        }

        string fileName = chara + "." + emotion;

        if (close)
        {
            CharacterManager.instance.ShowCloseCharacter(fileName);
        } else
        {
            CharacterManager.instance.HideCloseCharacter();
        }

        Sprite s = CharacterManager.instance.GetSprite(fileName + "." + "HEADSHOT");
        headshot.sprite = s;

        if (s == null)
        {
            headshot.color = new Color(1, 1, 1, 0);
        } else
        {
            headshot.color = new Color(1, 1, 1, 1);
        }
    }
}
