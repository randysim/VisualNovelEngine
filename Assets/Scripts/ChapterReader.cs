using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChapterReader : MonoBehaviour
{
    public static ChapterReader instance;

    public bool reading = false;
    private List<Section> sections;
    private int section = 0;

    private bool parsingLine = false;
    private GameObject voice = null; // queued voice

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

    public Command Next()
    {
        return sections[section].Next();
    }

    public void GoTo(string sec)
    {
        for (int i = 0; i < sections.Count; ++i)
        {
            if (sections[i].Name() == sec)
            {
                section = i;
                return;
            }
        }

        Debug.LogError("Invalid Section: " + sec);
    }

    public bool IsDone()
    {
        return sections[section].IsDone();
    }

    public void FinishReading()
    {
        DialogueSystem.instance.Disable();
        reading = false;
    }
    public void QueueVoice(string voiceName)
    {
        if (voice == null)
        {
            voice = SoundManager.instance.Play(voiceName);
        } else
        {
            Debug.LogError("Voice Already Queued.");
        }
    }
    public GameObject Voice()
    {
        return voice;
    }

    public void FinishReadingLine()
    {
        if (voice != null)
        {
            Destroy(voice);
            voice = null;
        }
    }

    public void LoadChapter(int n)
    {
        TextAsset chapterTxt = (TextAsset)Resources.Load($"chapters/chapter{n}", typeof(TextAsset));
        List<string> lines = new List<string>(chapterTxt.text.Split('\n'));
        List<Section> secs = new List<Section>();

        /* LOAD SECTIONS */
        int lastStart = 0;

        for (int i = 0; i < lines.Count; i++)
        {
            lines[i] = lines[i].Trim();
            if (lines[i].StartsWith("</SECTION>"))
            {
                secs.Add(new Section(lines.GetRange(lastStart, i - lastStart)));
            }
            else if (lines[i].StartsWith("<SECTION"))
            {
                lastStart = i;
            }
            
        }

        
        sections = secs;
        reading = true;

        GameManager.instance.OnChapterLoad();
    }

    public IEnumerator ReadLine()
    {
        parsingLine = true;

        Command line;
        
        do
        {
            line = Next();

            if (line != null) {
                float waitTime = line.Execute();

                if (waitTime < 0f)
                {
                    /* 
                     * THIS IS TO PAUSE THE FLOW ENTIRELY, EX: for choices
                     * USERS CAN NO LONGER MOVE FORWARD UNLESS OTHER ACTIONS ARE TAKEN
                     * AFTERWARDS, MAKE SURE TO set parsingLine = false
                     */
                    
                    yield break;
                }

                yield return new WaitForSeconds(waitTime);
                line.OnFinish(); // When command Finishes
                if (line.Name() == "READ") break;
            }
            
            /* SKIP OVER UNREGISTERED COMMANDS */
            while (line == null)
            {
                if (IsDone())
                {
                    break;
                }

                line = Next();
            }
        }
        while (line != null && !IsDone());

        parsingLine = false;
    }
    private void Update()
    {
        if (!reading || parsingLine) return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (DialogueSystem.instance.IsWriting())
            {
                DialogueSystem.instance.FinishWriting();
                return;
            }

            if (!IsDone())
            {
                StartCoroutine(ReadLine());
            }
            else
            {
                /* FINISHED READING CHAPTER */
                FinishReading();
            }
        }
    }

    // For commands that finish parsing outside of ChapterReader
    public void FinishParsing()
    {
        parsingLine = false;
    }
}
