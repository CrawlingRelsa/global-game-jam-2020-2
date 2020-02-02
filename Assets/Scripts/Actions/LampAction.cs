using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LampAction : PartAction
{
    [Header("Tutto ciò che vuoi mettere lo fai qui")]
    private Animator anim;

    public void Start()
    {
        anim = GetComponent<Animator>();
    }

    public override void HandleAction()
    {
        switch (currentStatus)
        {
            case 0:
                Change();
                break;
        }
        currentStatus++;
    }

    private void Change()
    {
        anim.StopPlayback();
        Destroy(anim);
        //TODO: qualunque cosa sia la luce
    }
}
