using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AiControls : MonoBehaviour
{
    Wizard _Wizard;
    private void Awake()
    {
        _Wizard = GetComponent<Wizard>();
        this.gameObject.tag = "Enemy";
        this.gameObject.layer = 6;
        GameObject AttackChecker = new GameObject("IncomingAttackChecker");
        AttackChecker.transform.parent = this.transform;
        AttackChecker.transform.localPosition = new Vector3(0, 2.88f, 0);
        AttackChecker.transform.rotation = Quaternion.identity;
        AttackChecker.AddComponent<SphereCollider>();
        AttackChecker.transform.localScale = new Vector3(8, 8, 8);
        AttackChecker.GetComponent<SphereCollider>().isTrigger = true;
        AttackChecker.gameObject.tag = this.gameObject.tag;
        AttackChecker.AddComponent<AttackChecker>();
        AttackChecker.GetComponent<AttackChecker>().aiControls = this;
        
    }

    private void Start()
    {
        _Wizard.IsPlayer = false;
        
        _Wizard.Enemy = FindObjectOfType<GameManager>().player.transform;
        
    }

    public void StartGame()
    {
        _Wizard.InIt();
        StartCoroutine(MakeDecision());
    }

    IEnumerator MakeDecision()
    {
        float decision = Random.value;
        if (decision < 0.33f)
        {
            if (_Wizard._isAlive)
            {
                Attack();
            }
        }
        else if(decision < 0.66f)
        {
            if(_Wizard._isAlive)
            {
                SpecialAttack();
            }
        }
        yield return new WaitForSeconds(1f);
        if(_Wizard._isAlive)
        {
            StartCoroutine(MakeDecision());
        }
    }

    public void BlockCheck()
    {
        float decision = Random.value;

        if (decision < 0.5f)
        {
            if(_Wizard._isAlive)
            {
                Block();
            }
        }
    }

    public void Attack()
    {
        _Wizard.BasicAttack();
    }
    public void Block()
    {
        _Wizard.Block();
    }
    public void SpecialAttack()
    {
        _Wizard.SpecialAttackCheck();
    }

}
