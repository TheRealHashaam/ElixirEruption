using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpeningCinematic : MonoBehaviour
{
    public Transform Startpos, EndPos;
    public ParticleSystem HitParticle;
    public ParticleSystem InstantiateParticle;
    public GameObject Flask;
    public float duration;
    public float SwitchAfter,finalduration;
    public GameObject CinematicCamera1, CinematicCamera2, CinematicCamera3, CinematicCamera4;
    GameManager gameManager;
    public AudioSource GlassShatter, PotionThrow;
    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
    }
    [ContextMenu("Cinematic")]
    public void StartCinematic()
    {
        Flask.SetActive(true);
        Flask.transform.position = Startpos.position;
        ThrowFlask();
    }

    public void ThrowFlask()
    {
        PotionThrow.Play();
        Flask.transform.DOMove(EndPos.position, duration).OnComplete(() => 
        {
            Flask.SetActive(false);
            GlassShatter.Play();
            PotionThrow.Stop();
            HitParticle.Play();
            StartCoroutine(FinalPart());
        }).SetEase(Ease.Linear);
        StartCoroutine(Delay());
    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(SwitchAfter);
        CinematicCamera1.SetActive(false);
        CinematicCamera2.SetActive(true);
    }

    IEnumerator FinalPart()
    {
        yield return new WaitForSeconds(finalduration);
        CinematicCamera3.SetActive(true);
        CinematicCamera2.SetActive(false);
        InstantiateParticle.Play();
        gameManager.GameplayMusic.Play();
        yield return new WaitForSeconds(0.6f);
        gameManager.PotionCheck();
        yield return new WaitForSeconds(0.5f);
        CinematicCamera3.SetActive(false);
        CinematicCamera4.SetActive(true);
        yield return new WaitForSeconds(1f);
        gameManager.BattleCamera.gameObject.SetActive(true);
        CinematicCamera3.SetActive(false);
        gameManager.OpenStartScreen();
    }

}
