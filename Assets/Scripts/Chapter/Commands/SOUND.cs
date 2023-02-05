using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SOUND : Command
{
    /* 
     ARGS:
    0!: NAME
    1!: SOUNDEFFECT
     */
    public SOUND (string c, Section s) : base(c, s)
    {

    }

    public override float Execute()
    {
        if (args.Length < 2)
        {
            Debug.LogError("Missing Sound Effect Name.");
            return 0f;
        }

        SoundManager.instance.Play(args[1]);
        return 0f;
    }
}
