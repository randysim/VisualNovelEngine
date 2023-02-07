using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Section
{
    private List<string> lines;
    private int progress = 0;
    private string name = "";

    public Section(List<string> lines)
    {
        this.lines = new List<string>();
        name = lines[0].Substring(1, lines[0].Length - 2).Split(' ')[1];
        lines = lines.GetRange(1, lines.Count - 1);

        /* GET RID OF ALL EMPTY LINES */
        for (int i = lines.Count - 1; i >= 0; i--)
        {
            if (lines[i] == "" || lines[i].StartsWith("#")) lines.RemoveAt(i);
        }

        bool parsingChoice = false;
        int choiceStart = -1;

        for (int i = 0; i < lines.Count; i++)
        {
            
            if (lines[i].StartsWith("<CHOICE"))
            {
                choiceStart = i;
                parsingChoice = true;
            }

            if (parsingChoice)
            {
                if (lines[i] == "</CHOICE>")
                {
                    this.lines.Add(string.Join('\n', lines.GetRange(choiceStart, i - choiceStart)));
                    parsingChoice = false;
                }

                continue;
            }

            this.lines.Add(lines[i]);
        }
    }

    public Command ParseCommand (string c)
    {
        if (c.StartsWith("${"))
        {
            return new VARIABLE(c, this);
        }

        if (c.StartsWith("<CHOICE"))
        {
            return new CHOICE(c, this);
        }

        c = GameManager.instance.ReplaceVariables(c);

        if (c.StartsWith("<"))
        {

            string name = c.Substring(1, c.Length - 2).Split(" ")[0];
            Type type = Type.GetType(name);

            return type == null ? new SKIP(c, this) : (Command)Activator.CreateInstance(type, c, this);
        } else 
        {
            // READ
            return new READ(c, this);
        }
    }

    public Command Next()
    {
        if (IsDone()) return null;

        Command line = ParseCommand(lines[progress]);

        progress++;
        return line;
    }

    public void InsertCommands(List<string> s)
    {
        for (int i = s.Count - 1; i >= 0; i--)
        {
            this.lines.Insert(this.progress, s[i]);
        }
    }

    public bool IsDone()
    {
        return lines.Count <= progress;
    }

    public string Name()
    {
        return name;
    }
}
