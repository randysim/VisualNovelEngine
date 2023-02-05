using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VariableSystem
{
    private Dictionary<string, string> variables = new Dictionary<string, string>();

    public void Set(string name, string value)
    {
        variables[name] = value;
    }

    public string Get(string name)
    {
        if (!variables.ContainsKey(name))
        {
            return null;
        }

        return variables[name];
    }

    // Load Variables from Save
    public void LoadVariables()
    {

    }

    public void Clear()
    {
        variables.Clear();
    }
}
