using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Issue
{
    public string statusName;
    public Tool.ToolType compatibleTool;

    public enum ActionType { Disappear }
    public ActionType actionType;
}
