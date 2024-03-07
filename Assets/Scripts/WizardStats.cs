using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WizardStats : MonoBehaviour
{
    float _maxHealth = 100;
    public float CurrentHealth;
    public  float _maxEnergy = 100;
    public float CurrentEnergy;
    public UIData WizardUI;
    private void Start()
    {
        CurrentHealth = _maxHealth;
        CurrentEnergy = 0;
        UpdateStats();
    }

    public void AddEnergy(int amount)
    {
        CurrentEnergy += amount;
        CurrentEnergy = Mathf.Clamp(CurrentEnergy, 0, _maxEnergy);
        if(CurrentEnergy==_maxEnergy)
        {
            WizardUI.EnergyMaxed();
        }
        UpdateStats();
    }

    public void TakeDamage(int amount)
    {
        CurrentHealth -= amount;
       // CurrentHealth = Mathf.Clamp(CurrentHealth, 0, _maxHealth);
        UpdateStats();
    }

    public void UpdateStats()
    {
        //Debug.LogError("here "+ CurrentEnergy);
       
        float healthfill = CurrentHealth / _maxHealth;
        healthfill = Mathf.Clamp(healthfill, 0f, 1f);
        float energyfill = CurrentEnergy / _maxEnergy;
        energyfill = Mathf.Clamp(energyfill, 0f, 1f);
        WizardUI.HealthBar.fillAmount = healthfill;
        WizardUI.EnergyBar.fillAmount = energyfill;
    }

}
