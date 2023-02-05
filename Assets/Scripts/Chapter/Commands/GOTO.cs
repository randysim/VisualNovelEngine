using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GOTO : Command
{
    /* 
     ARGS:
    0!: NAME
    1!: SECTION
     */

    private string sec;

    public GOTO(string c, Section s) : base(c, s)
    {
        sec = args[1];
    }

    public override float Execute()
    {
        ChapterReader.instance.GoTo(sec);

        return 0f;
    }
}
