using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Actions : MonoBehaviour
{
    [Header("Disappear")]
    public float disappearDuration = 0.3f;
    public iTween.EaseType disappearEaseType = iTween.EaseType.easeInOutCubic;


    #region Disappear
    public void Disappear(Transform target)
    {
        iTween.ScaleTo(target.gameObject, iTween.Hash(
            "scale", Vector3.zero,
            "time", disappearDuration,
            "easetype", disappearEaseType
        ));
    }
    #endregion
}
