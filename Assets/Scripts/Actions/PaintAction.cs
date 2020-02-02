using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintAction : PartAction
{
    [Header("Tutto ciò che vuoi mettere lo fai qui")]
    public AudioClip sprayClip;
    public GameObject carPainted;

    private GameObject child;
    private AudioSource paintSource;

    public void Start()
    {
        paintSource = GetComponent<AudioSource>();
        child = transform.GetChild(0).gameObject;
    }

    public override void HandleAction()
    {
        switch (currentStatus)
        {
            case 0:
                Paint();
                break;
        }
        currentStatus++;
    }

    private void Paint()
    {
        paintSource.clip = sprayClip;
        paintSource.Play();
        Destroy(child);
        child = Instantiate(carPainted, transform.position, transform.rotation, transform);
    }
}
