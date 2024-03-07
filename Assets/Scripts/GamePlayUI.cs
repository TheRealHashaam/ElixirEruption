using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePlayUI : MonoBehaviour
{
    public Image[] InGameUI;
    public float duration;
    public void ShowUI()
    {
        for (int i = 0; i < InGameUI.Length; i++)
        {
            InGameUI[i].DOFade(1, duration);
        }
    }
    public void HideUI()
    {
        for (int i = 0; i < InGameUI.Length; i++)
        {
            InGameUI[i].DOFade(0, duration);
        }
    }
}
