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
    private Color startingColor;

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
        StartCoroutine(FadeOut(paintSource, 1f));
        Vector3 oldScale = child.transform.localScale;
        Destroy(child);
        /* child = Instantiate(carPainted, transform.position, transform.rotation, transform);
        child.transform.localScale = oldScale;
        child.GetComponent<Material>().color = startingColor; */
    }
}
