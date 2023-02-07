using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class END : Command
{
    public END (string c, Section s) : base(c, s)
    {

    }

    public override float Execute()
    {
        GameManager.instance.OnGameEnd();

        return 0f;
    }
}
