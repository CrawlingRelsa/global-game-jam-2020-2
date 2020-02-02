using UnityEngine.Events;
using UnityEngine;
using System;

[System.Serializable]
public class Issue
{
    public Tool.ToolType compatibleTool;

    public void SolveIssue(Transform part)
    {
        Debug.Log("issue");
        part.GetComponent<PartAction>().HandleAction();
    }
}

