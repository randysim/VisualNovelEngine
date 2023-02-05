using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChoiceSystem : MonoBehaviour
{
    public static ChoiceSystem instance;

    [SerializeField]
    private GameObject choiceMenu;
    [SerializeField]
    private GameObject prompt;
    [SerializeField]
    private GameObject choices;
    [SerializeField]
    private GameObject choicePrefab;

    private List<GameObject> choiceButtons = new List<GameObject>();

    private TextMeshProUGUI promptText;

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

        promptText = prompt.GetComponent<TextMeshProUGUI>();
    }

    public void PromptChoice(string prompt, Section sec, List<string> choiceNames, List<List<string>> choiceResults)
    {
        promptText.text = prompt;

        for (int i = 0; i < choiceNames.Count; i++)
        {
            string name = choiceNames[i];
            List<string> result = choiceResults[i];

            GameObject choice = Instantiate(choicePrefab);
            choice.transform.SetParent(choices.transform);
            choiceButtons.Add(choice);

            Button btn = choice.GetComponent<Button>();
            TextMeshProUGUI btnText = btn.GetComponentInChildren<TextMeshProUGUI>();

            btnText.text = name;
            btn.onClick.AddListener(() => MakeChoice(result, sec));

            RectTransform rect = choice.GetComponent<RectTransform>();
            rect.anchoredPosition = new Vector2(0, -(i + 1) * (rect.rect.height * 1.5f));
        }

        choiceMenu.SetActive(true);
    }

    private void MakeChoice(List<string> result, Section sec)
    {
        sec.InsertCommands(result);
        ChapterReader.instance.FinishParsing();
        choiceMenu.SetActive(false);
        promptText.text = "";

        foreach (GameObject btn in choiceButtons)
        {
            Destroy(btn);
        }

        choiceButtons.Clear();
        StartCoroutine(ChapterReader.instance.ReadLine());
    }
}
