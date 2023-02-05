using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VOICE : Command
{
    /* 
     ARGS:
    0!: NAME
    1!: VOICE_NAME
     */

    public VOICE(string c, Section s) : base(c, s)
    {
        
    }

    public override float Execute()
    {
        ChapterReader.instance.QueueVoice(args[1]);

        return 0f;
    }
}
