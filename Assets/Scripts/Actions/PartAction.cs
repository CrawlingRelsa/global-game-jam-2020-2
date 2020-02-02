using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[RequireComponent(typeof (AudioSource))]
public abstract class PartAction : MonoBehaviour
{
    
    public int currentStatus = 0;
    public abstract void HandleAction();
    
}
