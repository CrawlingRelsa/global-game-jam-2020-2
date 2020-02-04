using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolDestroyer : MonoBehaviour
{
    public GameObject energyParticle;
    public float particleScaleMultiplier = 2f;
    public void OnTriggerEnter(Collider other)
    {
        Tool toolInside = other.gameObject.GetComponent<Tool>();
        if (toolInside)
        {
            ParticleSystem spawnedParticles = Instantiate(energyParticle, toolInside.transform.position, this.transform.rotation).GetComponent<ParticleSystem>();
            spawnedParticles.transform.localScale *= particleScaleMultiplier;
            if (spawnedParticles)
            {
                DestroyParticle(spawnedParticles);
            }            
            Destroy(toolInside.gameObject);
        }
    }

    public IEnumerator DestroyParticle(ParticleSystem particle)
    {
        yield return new WaitUntil(() => { return particle.isStopped;});
        Destroy(particle.gameObject);
    }
}
