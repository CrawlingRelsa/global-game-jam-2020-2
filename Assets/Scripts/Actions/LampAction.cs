using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LampAction : PartAction
{
    [Header("Tutto ciò che vuoi mettere lo fai qui")]
    public AudioClip lampClip;
    // TODO: qualunque cosa sia la luce

    private GameObject child;
    private AudioSource lampSource;

    public void Start()
    {
        lampSource = GetComponent<AudioSource>();
        child = transform.GetChild(0).gameObject;
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
        lampSource.clip = lampClip;
        lampSource.Play();
        //TODO: qualunque cosa sia la luce
    }
}
