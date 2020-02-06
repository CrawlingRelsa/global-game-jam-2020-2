using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tool : MonoBehaviour
{
    public enum ToolType { Spray, Screwdriver, Wheel, Hammer, Glass, FireExtinguisher, Brench, LightBulb, Condom }
    public ToolType toolType;
    public Store toolCreator;
    public bool isPermanent = true;
    private bool isQuitting = false;

    public bool IsCompatible(Part part) { return this.toolType == part.CurrentAction.compatibleTool; }

    public void DoAction(Part part)
    {
        part.Repair();
        if (!isPermanent)
        {
            Destroy(transform.gameObject);
        }
    }

    void OnApplicationQuit()
    {
        isQuitting = true;
    }
    void OnDestroy()
    {
        if (GameManager.Instance.remainingTime >= 0 && !isQuitting && toolCreator)
        {
            // Respawn this object
            toolCreator.CreateToolInstance();
        }
    }

}
