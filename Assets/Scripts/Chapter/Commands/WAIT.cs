using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WAIT : Command
{
    /* 
     ARGS:
    0!: NAME
    1!: TIME
     */

    private float time;

    public WAIT(string c, Section s) : base(c, s)
    {
        time = float.Parse(args[1]);
    }

    public override float Execute()
    {
        return time;
    }
}
