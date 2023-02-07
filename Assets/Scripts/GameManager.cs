using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Text.RegularExpressions;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public VariableSystem Variables;
    public bool DebugMode = true;

    public bool inChapter = true;

    [SerializeField]
    private GameObject credits;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;

        Variables = new VariableSystem();
    }

    private void Start()
    {
        /* 
         GameManager manages chapter loading later based on save, for now just load 1st
        */
        ChapterReader.instance.LoadChapter(1);
    }

    // replace instances of variables in a string with the value
    public string ReplaceVariables(string c)
    {
        var matches = Regex.Matches(c, "\\${.*?}");

        if (matches == null || matches.Count < 1)
        {
            return c;
        }
        for (int i = 0; i < matches.Count; i++)
        {
            string vName = matches[i].Value.Substring(2, matches[i].Value.Length - 3);

            c = c.Replace(
                matches[i].Value,
                Variables.Get(vName)
            );
        }

        return c;
    }

    public void OnChapterLoad()
    {
        Debug.Log("Chapter Loaded!");
        StartCoroutine(ChapterReader.instance.ReadLine());
    }

    public void OnGameEnd()
    {
        /* RUN CREDITS */
        credits.SetActive(true);
    }
}
