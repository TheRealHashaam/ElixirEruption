using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wand : MonoBehaviour
{
    public GameObject beamLineRendererPrefab;
    public GameObject beamStartPrefab;
    public GameObject beamEndPrefab;
    public ParticleSystem WandParticle;
    public ParticleSystem DomeParticle;
    public Transform WandOffset;
    public GameObject EndingImpact;
    public  GameObject BasicAttackParticle;
    public void Glow()
    {
        WandParticle.Play();
        WandParticle.GetComponent<Light>().enabled = true;
    }
    public void StopGlow()
    {
        WandParticle.Stop();
        WandParticle.GetComponent<Light>().enabled = false;
    }
    public void Attack()
    {

    }
}
