using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EngineAction : PartAction
{
    [Header("Tutto ciò che vuoi mettere lo fai qui")]
    public AudioClip extinguishClip;
    public AudioClip wrenchClip;
    public GameObject smokeParticles;

    //
    private GameObject child;
    private AudioSource engineSource;
    //
    Transform oldPosition;


    public void Start()
    {
        engineSource = GetComponent<AudioSource>();
        child = GetComponentInChildren<ParticleSystem>().gameObject;
        oldPosition = child.transform;
    }

    public override void HandleAction()
    {
        switch (currentStatus)
        {
            case 0:
                Extinguish();
                break;
            case 1:
                Repair();
                break;

        }
        currentStatus++;

    }

    private void Repair()
    {
        engineSource.clip = wrenchClip;
        engineSource.Play();
        Destroy(child);
    }

    private void Extinguish()
    {
        engineSource.clip = extinguishClip;
        engineSource.Play();
        Destroy(child);
        child = Instantiate(smokeParticles, oldPosition.position, oldPosition.rotation, transform);
    }
}
