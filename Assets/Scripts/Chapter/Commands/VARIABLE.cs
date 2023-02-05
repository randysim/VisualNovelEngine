using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VARIABLE : Command
{
    /* ALL VARIABLES WILL BE STORED IN STRINGS. DEPENDING ON OPERATOR, TEMP FLOAT CONVERSIONS
     ARGS:
    0!: ${VARIABLE NAME}
    1!: OPERATOR
    2!: VALUE
     */

    public VARIABLE(string c, Section s) : base(c, s)
    {
        // replace args. no brackets
        args = c.Split(' ');
    }

    public override float Execute()
    {
        string vName = args[0].Substring(2, args[0].Length - 3);

        string vValue = GameManager.instance.Variables.Get(vName);

        string vValue2 = args[2];
        if (args[2].StartsWith("${"))
        {
            string v2Name = args[2].Substring(2, args[2].Length - 3);
            // fetch from global variable manager
            vValue2 = GameManager.instance.Variables.Get(v2Name);
        }

        switch (args[1])
        {
            case "+=":
                vValue = (float.Parse(vValue) + float.Parse(vValue2)).ToString();
                break;
            case "-=":
                vValue = (float.Parse(vValue) - float.Parse(vValue2)).ToString();
                break;
            case "*=":
                vValue = (float.Parse(vValue) * float.Parse(vValue2)).ToString();
                break;
            case "/=":
                vValue = (float.Parse(vValue) / float.Parse(vValue2)).ToString();
                break;
            case "=":
                vValue = vValue2;
                break;
        }

        // Set to Global Variable Manager
        GameManager.instance.Variables.Set(vName, vValue);

        return 0f;
    }
}
