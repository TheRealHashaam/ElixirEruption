using UnityEngine;
using System.Collections;
 
public class MagicProjectileScript : MonoBehaviour
{
    public GameObject impactParticle;
    public GameObject projectileParticle;
    public GameObject muzzleParticle;
    public GameObject[] trailParticles;
    [HideInInspector]
    public Vector3 impactNormal; //Used to rotate impactparticle.
 
    private bool hasCollided = false;
 
    void Start()
    {
        projectileParticle = Instantiate(projectileParticle, transform.position, transform.rotation) as GameObject;
        projectileParticle.transform.parent = transform;
		if (muzzleParticle){
        muzzleParticle = Instantiate(muzzleParticle, transform.position, transform.rotation) as GameObject;
        Destroy(muzzleParticle, 1.5f); // Lifetime of muzzle effect.
		}
    }
    
    public void Collided()
    {

        hasCollided = true;
        //transform.DetachChildren();
        impactParticle = Instantiate(impactParticle, transform.position, Quaternion.FromToRotation(Vector3.up, impactNormal)) as GameObject;

        foreach (GameObject trail in trailParticles)
        {
            GameObject curTrail = transform.Find(projectileParticle.name + "/" + trail.name).gameObject;
            curTrail.transform.parent = null;
            Destroy(curTrail, 3f);
        }
        Destroy(projectileParticle, 3f);
        Destroy(impactParticle, 3f);
        Destroy(gameObject);
        //projectileParticle.Stop();

        ParticleSystem[] trails = GetComponentsInChildren<ParticleSystem>();
        //Component at [0] is that of the parent i.e. this object (if there is any)
        for (int i = 1; i < trails.Length; i++)
        {
            ParticleSystem trail = trails[i];
            if (!trail.gameObject.name.Contains("Trail"))
                continue;

            trail.transform.SetParent(null);
            Destroy(trail.gameObject, 2);

        }
    }

    void OnCollisionEnter(Collision hit)
    {
        if (!hasCollided)
        {
            
            if (hit.gameObject.tag ==this.gameObject.tag)
            {
                return;
            }
            
            if(hit.gameObject.GetComponent<Wizard>())
            {
                Debug.LogError("Hit " + hit.gameObject.name);
                hit.gameObject.GetComponent<Wizard>().Damage(8);
            }

            if (hit.gameObject.tag == "Destructible") // Projectile will destroy objects tagged as Destructible
            {
                Destroy(hit.gameObject);
            }

            Collided();
        }
    }
}