using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HIDE : Command
{
    /* 
     ARGS:
    0!: NAME
    1!: [CHARACTERS]
    2?: TRANSITION
    3?: DURATION
    */

    public HIDE(string c, Section s) : base(c, s)
    {
        
    }

    public override float Execute()
    {
        string transition = "INSTANT";
        float duration = 0f;

        string[] characters = args[1].Split(',');

        if (args.Length > 2)
        {
            transition = args[2];
            duration = 0.5f; // default duration
            if (args.Length > 3) duration = float.Parse(args[3]);
        }

        for (int i = 0; i < characters.Length; ++i)
        {
            CharacterManager.instance.HideCharacter(characters[i], transition, duration);
        }

        return 0;
    }
}