using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TyreAction : PartAction
{
    [Header("Tutto ciò che vuoi mettere lo fai qui")]
    public AudioClip wrenchClip;
    public AudioClip screwClip;
    public GameObject repaired;
    public float disappearDuration = 0.3f;
    public iTween.EaseType disappearEaseType = iTween.EaseType.easeInOutCubic;


    //
    private GameObject child;
    private Vector3 initialScale;
    private AudioSource tyreSource;

    public void Start()
    {
        tyreSource = GetComponent<AudioSource>();
        child = transform.GetChild(0).gameObject;
        initialScale = transform.localScale;
    }

    public override void HandleAction()
    {
        switch (currentStatus)
        {
            case 0:
                Shrink();
                break;
            case 1:
                Replace();
                break;

        }
        currentStatus++;

    }

    private void Replace()
    {
        tyreSource.clip = screwClip;
        tyreSource.Play();
        Destroy(child);
        child = Instantiate(repaired, transform.position, transform.rotation, transform);
        child.transform.localScale = Vector3.zero;
        iTween.ScaleTo(child.gameObject, iTween.Hash(
            "scale", initialScale,
            "time", disappearDuration,
            "easetype", disappearEaseType,
            "oncompletetarget", child.gameObject,
            "oncompleteparams", transform
        ));

    }

    private void Shrink()
    {
        tyreSource.clip = wrenchClip;
        tyreSource.Play();
        iTween.ScaleTo(child.gameObject, iTween.Hash(
            "scale", Vector3.zero,
            "time", disappearDuration,
            "easetype", disappearEaseType,
            "oncompletetarget", child.gameObject,
            "oncompleteparams", transform
        ));
    }
}
