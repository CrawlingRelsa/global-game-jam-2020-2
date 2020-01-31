using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITool
{
    bool IsCompatible (Part part);
    void DoAction(Part part);
}
