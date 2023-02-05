using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UNCLOSE : Command
{
    public UNCLOSE (string c, Section s) : base(c, s)
    {

    }

    public override float Execute()
    {
        CharacterManager.instance.HideCloseCharacter();

        return 0f;
    }
}
