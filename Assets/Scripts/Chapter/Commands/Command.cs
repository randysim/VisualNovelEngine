using UnityEngine;

public abstract class Command
{
    protected string name;
    protected string[] args;
    protected Section section;

    public Command(string c, Section s) {
        section = s;

        if (!c.StartsWith("<"))
        {
            // READ
            name = "READ";
            return;
        }
        args = c.Trim().Substring(1, c.Length - 2).Split(" ");
        name = args[0];
    }

    public string Name ()
    {
        return name;
    }

    public virtual void OnFinish()
    {
        if (GameManager.instance.DebugMode)
        {
            Debug.Log(Name() + " finished running!");
        }
    }

    /* RETURNS WAIT TIME IN SECONDS */
    public abstract float Execute();
}
