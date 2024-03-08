using Cinemachine;
using DG.Tweening;
using MagicArsenal;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Wizard : MonoBehaviour
{
    Animator animator;
    public AudioSource SpellCast, SpellImpact;
    private GameObject beamStart;
    private GameObject beamEnd;
    private GameObject beam;
    private LineRenderer line;
    public float beamEndOffset = 1f;
    public float textureScrollSpeed = 8f; 
    public float textureLengthScale = 3;
    public Wand wand;
    public float ParticleDelay, FireDuration;
    public float AttackHoldDelay;
    GameManager _gameManager;
    IKControl iK;
    public Vector3 EndingOffset;
    public Color SpellColor;
    int _attackNo=0;
    bool _canAttack = true;
    bool _canBlock = true;
    public GameObject SphereObject;
    public Transform Enemy;
    public float ProjectileSpeed;
    public Vector3 HitOffset;
    public WizardStats _stats;
    public CinemachineImpulseSource impulse;
    public bool IsPlayer;
    public bool _isAlive = true;
    bool _specialAttack;
    public AudioSource AttackSound;
    public AudioClip sound1, sound2;
    public void InIt()
    {
        animator = GetComponent<Animator>();
        _gameManager = FindObjectOfType<GameManager>();
        iK = GetComponent<IKControl>();
        SphereObject.gameObject.tag = this.gameObject.tag;
        if (IsPlayer)
        {
            impulse = _gameManager.BattleCamera.GetComponent<CinemachineImpulseSource>();
        }
        _isAlive = true;
        StopTaunt();
    }
    public void AttackState()
    {
        _canAttack = true;
    }

    public void BlockState()
    {
        _canBlock = true;
    }

    public void PlayTaunt()
    {
        animator.SetBool("Taunt", true);
    }

    public void StopTaunt()
    {
        animator.SetBool("Taunt", false);
    }

    public void Damage(int amount)
    {
        if(_isAlive)
        {
            if(!_specialAttack)
            {
                _stats.TakeDamage(amount);
                if (_stats.CurrentHealth>0)
                {
                    animator.SetTrigger("Hit");
                    _canAttack = true;
                }
                else
                {
                    Death();
                }
                _stats.UpdateStats();
            }
        }
    }

    public void Electric()
    {
        animator.SetTrigger("SpecialHit");
    }

    #region Attack
    public void BasicAttack()
    {
        if(_canAttack)
        {
            _canAttack = false;
            if (_attackNo==0)
            {
                _attackNo = 1;
                Attack1();
            }
            else
            {
                _attackNo = 0;
                Attack2();
            }
            _stats.AddEnergy(10);
        }
    }

    [ContextMenu("Basic Attack1")]
    public void Attack1()
    {
        animator.SetTrigger("Attack1");
        if(IsPlayer)
        {
            AttackSound.clip = sound1;
            AttackSound.Play();
        }

    }
    [ContextMenu("Basic Attack2")]
    public void Attack2()
    {
        animator.SetTrigger("Attack2");
        if(IsPlayer)
        {
            AttackSound.clip = sound2;
            AttackSound.Play();
        }
    }

    public void AttackParticle()
    {
        GameObject projectile = Instantiate(wand.BasicAttackParticle, wand.WandOffset.position, Quaternion.identity) as GameObject;
        projectile.tag = this.tag;
        projectile.layer = this.gameObject.layer;
        projectile.transform.LookAt(Enemy);
        projectile.GetComponent<Rigidbody>().AddForce(projectile.transform.forward * ProjectileSpeed+ Enemy.GetComponent<Wizard>().HitOffset);
        projectile.GetComponent<MagicProjectileScript>().impactNormal = Enemy.position;
        if(impulse)
        {
            impulse.GenerateImpulse();
        }
    }


    public void SpecialAttackCheck()
    {
        if(_stats.CurrentEnergy== _stats._maxEnergy)
        {
            ChargeAttack();
        }
    }

    [ContextMenu("SpecialAttack")]
    public void ChargeAttack()
    {
        _specialAttack = true;
        _stats.CurrentEnergy = 0;
        animator.SetTrigger("SpecialAttack");
        SpellCast.Play();
        wand.Glow();
        _gameManager.SetActor(this.transform);
        _gameManager.BattleCamera.SetActive(false);
        _gameManager.SpecialAttackCamera.SetActive(true);
        StartCoroutine(Attack_Delay());
    }
    IEnumerator Attack_Delay()
    {
        yield return new WaitForSeconds(AttackHoldDelay);
        SpecialAttack();
        _gameManager.BattleCamera.SetActive(true);
        _gameManager.SpecialAttackCamera.SetActive(false);
    }

    [System.Obsolete]
    public void SpecialAttack()
    {
        animator.SetTrigger("Fire");
        StartCoroutine(Fire_Delay());
    }

    public void SpecialAttackFire()
    {
        iK.ikActive = true;
        Enemy.GetComponent<Wizard>().Electric();
        beamStart = Instantiate(wand.beamStartPrefab, wand.WandOffset.position, Quaternion.identity) as GameObject;
        wand.StopGlow();
        for (int i = 0; i < beamStart.GetComponent<ParticleData>().particleSystems.Length; i++)
        {
            beamStart.GetComponent<ParticleData>().particleSystems[i].startColor = SpellColor;
        }
        beamStart.GetComponentInChildren<MagicLightFlicker>().originalColor = SpellColor;
        beamEnd = Instantiate(wand.beamEndPrefab, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
        for (int i = 0; i < beamEnd.GetComponent<ParticleData>().particleSystems.Length; i++)
        {
            beamEnd.GetComponent<ParticleData>().particleSystems[i].startColor = SpellColor;
        }
        beamEnd.GetComponentInChildren<MagicLightFlicker>().originalColor = SpellColor;
        beam = Instantiate(wand.beamLineRendererPrefab, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
        line = beam.GetComponent<LineRenderer>();
        line.startColor = new Color(SpellColor.r, SpellColor.g, SpellColor.b, 0);
        line.endColor = SpellColor;
        Vector3 tdir = Enemy.position - transform.position;
        ShootBeamInDir(tdir);
        _gameManager.Shake();

        _stats.WizardUI.EnergyBar.DOFillAmount(0, ParticleDelay).OnComplete(() =>
        {

            _stats.WizardUI.EnergyNormalized();
        });
        StartCoroutine(Destroy_Delay());
    }

    [System.Obsolete]
    IEnumerator Fire_Delay()
    {
        yield return new WaitForSeconds(ParticleDelay);
        iK.ikActive = true;
        yield return new WaitForSeconds(0.1f);
        Enemy.GetComponent<Wizard>().Electric();
        beamStart = Instantiate(wand.beamStartPrefab, wand.WandOffset.position,Quaternion.identity) as GameObject;
        wand.StopGlow();
        for (int i = 0; i < beamStart.GetComponent<ParticleData>().particleSystems.Length; i++)
        {
            beamStart.GetComponent<ParticleData>().particleSystems[i].startColor = SpellColor;
        }
        beamStart.GetComponentInChildren<MagicLightFlicker>().originalColor = SpellColor;
        beamEnd = Instantiate(wand.beamEndPrefab, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
        for (int i = 0; i < beamEnd.GetComponent<ParticleData>().particleSystems.Length; i++)
        {
            beamEnd.GetComponent<ParticleData>().particleSystems[i].startColor = SpellColor;
        }
        beamEnd.GetComponentInChildren<MagicLightFlicker>().originalColor = SpellColor;
        beam = Instantiate(wand.beamLineRendererPrefab, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
        line = beam.GetComponent<LineRenderer>();
        line.startColor = new Color(SpellColor.r, SpellColor.g, SpellColor.b, 0);
        line.endColor = SpellColor;
        Vector3 tdir = Enemy.position - transform.position;
        ShootBeamInDir(tdir);
        _gameManager.Shake();
        
        _stats.WizardUI.EnergyBar.DOFillAmount(0, ParticleDelay).OnComplete(()=> 
        {
            
            _stats.WizardUI.EnergyNormalized();
        });
        StartCoroutine(Destroy_Delay());
    }
    IEnumerator Destroy_Delay()
    {
        yield return new WaitForSeconds(FireDuration);
        animator.SetTrigger("SpellComplete");
        Destroy(beamStart);
        Enemy.GetComponent<Wizard>().Damage(50);
        GameObject g = Instantiate(wand.EndingImpact, beamEnd.transform.position,Quaternion.identity);
        for (int i = 0; i < g.GetComponent<ParticleData>().particleSystems.Length; i++)
        {
            g.GetComponent<ParticleData>().particleSystems[i].startColor = SpellColor;
        }
        Destroy(beamEnd);
        Destroy(beam);
        iK.ikActive = false;
        _gameManager.StopShake();
        Destroy(g, 2f);
        SpellImpact.Play();
        _specialAttack = false;
    }
    public float BeamDistance;
    public void ShootBeamInDir(Vector3 dir)
    {
#if UNITY_5_5_OR_NEWER
        line.positionCount = 2;
#else
		line.SetVertexCount(2); 
#endif
        line.SetPosition(0, wand.WandOffset.position);
        beamStart.transform.position = wand.WandOffset.position;
        Vector3 end = Vector3.zero;

        end = wand.WandOffset.position + (dir * BeamDistance);

        beamEnd.transform.position = end + EndingOffset;
        line.SetPosition(1, end+ EndingOffset);

        beamStart.transform.LookAt(beamEnd.transform.position);
        beamEnd.transform.LookAt(beamStart.transform.position);

        float distance = Vector3.Distance(wand.WandOffset.position, end);
        line.sharedMaterial.mainTextureScale = new Vector2(distance / textureLengthScale, 1);
        line.sharedMaterial.mainTextureOffset -= new Vector2(Time.deltaTime * textureScrollSpeed, 0);
    }
    #endregion

    #region Block

    public void Block()
    {
        if(_canBlock)
        {
            _canBlock = false;
            animator.SetTrigger("Block");
        }
    }

    public void ShowBlockParticle()
    {
        wand.DomeParticle.Play();
        SphereObject.SetActive(true);
        StartCoroutine(Block_Normalize());
    }
    IEnumerator Block_Normalize()
    {
        yield return new WaitForSeconds(1);
        wand.DomeParticle.Stop();
        SphereObject.SetActive(false);
    }

    #endregion

    public void Death()
    {
        _isAlive = false;
        animator.SetTrigger("Death");
        _gameManager.Winner = Enemy.GetComponent<Wizard>();
        Invoke("Complete", 2f);
    }

    void Complete()
    {
        _gameManager.WinCinematic();
    }

}
