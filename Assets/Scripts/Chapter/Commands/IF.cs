using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IF : Command
{
    /* 
     ARGS:
    0! NAME
    1! FIRST VALUE
    2! OPERATOR (<, >, =)
    3! SECOND VALUE
     */
    public IF (string c, Section s) : base(c, s)
    {

    }

    
    public override float Execute()
    {
        bool result = false;

        /* 
         SECTION automatically replaced variables with actual values
        */
        string val1 = args[1];
        string val2 = args[3];

        switch(args[2])
        {
            case ">":
                result = float.Parse(val1) > float.Parse(val2);
                break;
            case "<":
                result = float.Parse(val1) < float.Parse(val2);
                break;
            case "=":
                result = val1 == val2;
                break;
        }

        if (result) return 0f;

        while (section.Next().Name() != "/IF") ;

        return 0f;
    }
}
