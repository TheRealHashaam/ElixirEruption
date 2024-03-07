using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blocker : MonoBehaviour
{
    WizardStats _wizardStats;
    private void Awake()
    {
        _wizardStats = GetComponentInParent<WizardStats>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != this.gameObject.tag)
        {
            if (other.gameObject.GetComponent<MagicProjectileScript>())
            {
                other.gameObject.GetComponent<MagicProjectileScript>().Collided();
                _wizardStats.AddEnergy(20);
            }

        }
    }
}
