using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackChecker : MonoBehaviour
{
    public AiControls aiControls;
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            aiControls.BlockCheck();
        }
    }
}
