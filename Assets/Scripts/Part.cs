using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Part : MonoBehaviour
{
    public List<Issue> issues = new List<Issue>();

    public Issue CurrentAction
    {
        get
        {
            return issues[0];
        }
    }

    public void Repair()
    {
        Actions.Apply(CurrentAction.actionType, transform);
        issues.RemoveAt(0);
    }
}
