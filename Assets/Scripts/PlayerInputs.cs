using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputs : MonoBehaviour
{
    Wizard _Wizard;
    private void Awake()
    {
        _Wizard = GetComponent<Wizard>();
        this.gameObject.tag = "Player";
        this.gameObject.layer = 3;
    }
    private void Start()
    {
        _Wizard.IsPlayer = true;
        _Wizard.Enemy = FindObjectOfType<GameManager>().enemy.transform;
        
    }

    public void StartGame()
    {
        _Wizard.InIt();
    }

    public void OnAttack()
    {
        if(_Wizard._isAlive)
        {
            _Wizard.BasicAttack();
        }
    }
    public void OnBlock()
    {
        if(_Wizard._isAlive)
        {
            _Wizard.Block();
        }
    }
    public void OnSpecialAttack()
    {
        if(_Wizard._isAlive)
        {
            _Wizard.SpecialAttackCheck();
        }
    }
}
