using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Part : MonoBehaviour
{
    public Car car;
    public float points;
    public List<Issue> issues = new List<Issue>();

    public Issue CurrentAction
    {
        get
        {
            if (issues.Count > 0)
                return issues[0];
            return null;
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
