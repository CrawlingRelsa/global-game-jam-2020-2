using UnityEngine.Events;
using UnityEngine;
using System;

[System.Serializable]
public class Issue
{
    public string statusName;
    public Tool.ToolType compatibleTool;
    [SerializeField]
    public IssueEvent action;

    public void SolveIssue(Transform part)
    {
        //questa indica l'animazione
        action.Invoke(part);
    }

    [Serializable]
    public class IssueEvent : UnityEvent<Transform> { }
}

