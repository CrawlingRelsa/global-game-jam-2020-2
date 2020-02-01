using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PartAction : MonoBehaviour
{
    public int currentStatus = 0;
    public abstract void HandleAction();
}
