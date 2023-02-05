using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// SIMPLY SKIPS. NOT FOR WRITER.
public class SKIP : Command
{
    public SKIP(string c, Section s) : base(c, s)
    {

    }

    public override float Execute()
    {
        return 0f;
    }

}
