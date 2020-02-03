using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tool : MonoBehaviour
{
    public enum ToolType { Spray, Screwdriver, Wheel, Hammer, Glass, FireExtinguisher, Brench, LightBulb, Condom }
    public ToolType toolType;
    public bool isPermanent = true;

    public bool IsCompatible(Part part) { return this.toolType == part.CurrentAction.compatibleTool; }

    public void DoAction(Part part)
    {
        part.Repair();
    }
}
