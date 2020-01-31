using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Actions
{
    public static void Apply(Issue.ActionType actionType, Transform target)
    {
        switch (actionType)
        {
            case Issue.ActionType.Disappear:
                Disappear(target);
                break;
            default:
                return;
        }
    }


    public static float disappearDuration = 0.3f;
    public static iTween.EaseType disappearEaseType = iTween.EaseType.easeInOutCubic;
    private static void Disappear(Transform target)
    {
        iTween.ScaleTo(target.gameObject, iTween.Hash(
            "scale", Vector3.zero,
            "time", disappearDuration,
            "easetype", disappearEaseType
        ));
    }
}
