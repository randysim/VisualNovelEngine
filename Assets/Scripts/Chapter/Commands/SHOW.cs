using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SHOW : Command
{
    /* 
     ARGS:
    0!: NAME
    1!: [CHARACTERS.STATE?...]
    2!: [FLOAT...] (indexes)
    3?: TRANSITION
    4?: DURATION
     */

    public SHOW(string c, Section s) : base(c, s)
    {
        
    }

    public override float Execute()
    {
        DialogueSystem.instance.Disable();

        string transition = "INSTANT";
        float duration = 0f;

        string[] characters = args[1].Split(',');
        string[] indexes = args[2].Split(',');
        
        if (args.Length > 3)
        {
            transition = args[3];
            duration = 0.5f; // default duration
            if (args.Length > 4) duration = float.Parse(args[4]);
        }

        for (int i = 0; i < characters.Length; ++i)
        {
            CharacterManager.instance.ShowCharacter(float.Parse(indexes[i]), characters[i], transition, duration);
        }

        return 0;
    }
}
