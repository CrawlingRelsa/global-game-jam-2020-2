using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Part : MonoBehaviour
{
    public Car car;
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
        if (issues.Count > 0)
        {
            issues[0].SolveIssue(transform);
            issues.RemoveAt(0);
        }

    }
}
