using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 
 
 */
public class BGMUSIC : Command
{
    /* 
     ARGS:
    BGMUSIC PLAY audio? transition? duration?
    BGMUSIC PAUSE transition? duration?
    BGMUSIC END transition? duration?
    BGMUSIC RESUME transition? duration?

    always returns 0 duration. Should not pause game for music transitions
     */

    public BGMUSIC (string c, Section s) : base(c, s)
    {
        
    }

    public override float Execute()
    {
        if (args[1] == "PLAY")
        {
            string source = args[2];
            string transition = "INSTANT";
            float duration = 0f;

            if (args.Length > 3)
            {
                transition = args[3];
                duration = 0.5f;
                if (args.Length > 4)
                {
                    duration = float.Parse(args[4]);
                }
            }

            MusicManager.instance.Play(source, transition, duration);
        } else if (args[1] == "PAUSE" || args[1] == "END" || args[1] == "RESUME")
        {
            string transition = "INSTANT";
            float duration = 0f;

            if (args.Length > 2)
            {
                transition = args[2];
                duration = 0.5f;
                if (args.Length > 3)
                {
                    duration = float.Parse(args[3]);
                }
            }

            switch(args[1])
            {
                case "PAUSE":
                    MusicManager.instance.Pause(transition, duration);
                    break;
                case "END":
                    MusicManager.instance.End(transition, duration);
                    break;
                case "RESUME":
                    MusicManager.instance.Resume(transition, duration);
                    break;
            }
        }  else
        {
            Debug.LogError("Invalid BGMUSIC Command: " + args[1]);
        }

        return 0f;
    }
}
