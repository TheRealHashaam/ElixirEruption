using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionPanelData : MonoBehaviour
{
    public PotionSlot[] Slots;

    public DragAndDrop[] Potions;

    public List<DragAndDrop> CurrentPotions;
    int count = 0;

    public int PotionsAllowed = 0;
    public AudioSource PotionSound;
    GameManager manager;
    private void Start()
    {
        for (int i = 0; i < PotionsAllowed; i++)
        {
            Potions[i].gameObject.SetActive(true);
        }
        manager = FindObjectOfType<GameManager>();
    }

    public void ResetAll()
    {
        for (int i = 0; i < Slots.Length; i++)
        {

        }
        for (int i = 0;i < Potions.Length; i++)
        {
            Potions[i].Reset();
        }
        count = 0;
        CurrentPotions.Clear();
    }

    public void AddPotion(DragAndDrop temp)
    {
        CurrentPotions.Add(temp);
        count++;
        PotionSound.Play();
        if (count==2)
        {
            StartCoroutine(Cinematic_Delay());
            
        }
    }

    IEnumerator Cinematic_Delay()
    {
        yield return new WaitForSeconds(0.7f);
        manager.ClosePotionPanel();
        manager.GoToCinematic();
    }

}
