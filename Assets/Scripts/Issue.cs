using UnityEngine.Events;
using UnityEngine;
using System;

[System.Serializable]
public class Issue
{
    public Tool.ToolType compatibleTool;
    public GameObject repairedObject;
    [SerializeField]
    public IssueEvent action;

    public void SolveIssue(Transform part)
    {
        //Debug.Log("BOOM BABY");
        //questa indica l'animazione
        action.Invoke(part);
    }

    [Serializable]
    public class IssueEvent : UnityEvent<Transform> { }
}

