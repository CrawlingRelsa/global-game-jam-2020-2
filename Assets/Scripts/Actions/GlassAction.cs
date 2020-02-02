using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlassAction : PartAction
{
    [Header("Tutto ciò che vuoi mettere lo fai qui")]
    public AudioClip breakClip;
    public AudioClip hammerClip;
    public GameObject repairedGlass;

    //
    private GameObject child;
    private AudioSource glassSource;
    //private ParticleSystem breakParticles;

    public void Start()
    {
        //breakParticles = GetComponent<ParticleSystem>();
        glassSource = GetComponent<AudioSource>();
        child = transform.GetChild(0).gameObject;
    }

    public override void HandleAction()
    {
        switch (currentStatus)
        {
            case 0:
                Break();
                break;
            case 1:
                Replace();
                break;

        }
        currentStatus++;

    }

    private void Replace()
    {
        glassSource.clip = hammerClip;
        glassSource.Play();
        
        child = Instantiate(repairedGlass, transform.position, transform.rotation, transform);

    }

    private void Break()
    {
        glassSource.clip = breakClip;
        glassSource.Play();
        //breakParticles.Play();
        Destroy(child);
    }
}
