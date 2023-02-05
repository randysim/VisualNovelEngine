using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BG : Command
{
    /* 
     ARGS:
    0!: NAME
    1!: BACKGROUND
    2?: TRANSITION
    3?: TRANSITION DURATION
     */
    private ScreenState screenState;

    public BG (string c, Section s) : base(c, s)
    {
        screenState = ScreenState.instance;
    }

    public override float Execute()
    {
        DialogueSystem.instance.Disable();

        if (args.Length < 2)
        {
            // DEFAULT FADE OUT 0.5s
            BackgroundManager.instance.HideBackground("FADE", 0.5f);
        }
        else
        {
            string oldBackground = "";
            if (BackgroundManager.instance.currentBackground != null) 
                oldBackground = BackgroundManager.instance.currentBackground.name;

            if (oldBackground != args[1])
            {
                // update screen
                string transition = "INSTANT";
                float duration = 0f;
                if (args.Length >= 3)
                {
                    transition = args[2];
                    duration = 0.5f; // default duration
                    if (args.Length >= 4)
                    {
                        duration = float.Parse(args[3]);
                    }
                }

                BackgroundManager.instance.SetBackground(args[1], transition, duration);
            }
        }

        return 0f;
    }
}
