using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIData : MonoBehaviour
{
    public Image HealthBar;
    public Image EnergyBar;
    public Color EnergyMaxedColor;
    public Color NormalEnergyColor;
    public void EnergyMaxed()
    {
        EnergyBar.color = EnergyMaxedColor;
    }
    public void EnergyNormalized()
    {
        EnergyBar.color = NormalEnergyColor;
    }

}
