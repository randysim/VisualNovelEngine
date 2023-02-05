using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 
 Once picked choice, inserts commands into section Next() queue.
 */

public class CHOICE : Command
{
    private string prompt = "";

    private List<string> choiceNames = new List<string>();
    private List<List<string>> choiceResults = new List<List<string>>();

    /* 
     STARTS AT <CHOICE name>
    ENDS BEFORE </CHOICE> (</CHOICE> is not the last line)

    ARGS:
    0!: NAME
    REST: prompt
     */
    public CHOICE(string c, Section s) : base(c.Split("\n")[0], s)
    {
        List<string> lines = new List<string>(c.Split("\n"));

        for (int i = 1; i < args.Length; i++)
        {
            prompt += args[i];
            if (i != args.Length - 1)
            {
                prompt += " ";
            }
        }

        int optionStart = -1;
        for (int i = 1; i < lines.Count; i++)
        {
            if (lines[i].StartsWith("<OPTION")) optionStart = i;
            if (lines[i] == "</OPTION>")
            {
                string name = "";

                string[] optionArgs = lines[optionStart].Substring(1, lines[optionStart].Length - 2).Split(" ");
                for (int j = 1; j < optionArgs.Length; j++)
                {
                    name += optionArgs[j] + " ";
                }
                name = name.Trim();

                choiceNames.Add(GameManager.instance.ReplaceVariables(name));
                choiceResults.Add(lines.GetRange(optionStart + 1, i - optionStart - 1));
            }
        }

        prompt = GameManager.instance.ReplaceVariables(prompt);
    }

    public override float Execute()
    {
        ChoiceSystem.instance.PromptChoice(prompt, section, choiceNames, choiceResults);

        return -1f;
    }
}
