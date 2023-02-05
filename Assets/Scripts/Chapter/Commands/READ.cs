using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class READ : Command
{
    private string text;

    public READ(string c, Section s) : base(c, s)
    {
        text = c;
    }

    public override float Execute()
    {
        DialogueSystem.instance.Talk(text);

        return 0f;
    }
}
