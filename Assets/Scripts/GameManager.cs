using Cinemachine;
using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using static DragAndDrop;
using UnityEngine.InputSystem;
using StarterAssets;
using UnityEngine.Windows;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject BattleCamera;
    public GameObject PlayerCamera;
    public GameObject SpecialAttackCamera;
    public GameObject WinCameraPlayer, WinCameraAI;
    CinemachineVirtualCamera _specialAttackCamera;
    public Wizard player, enemy;
    CinemachineBasicMultiChannelPerlin Perlin;
    public UIData PlayerUI, EnemyUI;
    public Wizard Winner;
    public float TauntDelay;
    public GamePlayUI GameplayUI;
    public Image FadePanel;
    public PotionPanelData PotionPanelData;
    public AudioSource ClickSound;
    public GameObject MainmenuCamera, MainMenuPanel;
    public PlayerInput _input;
    public GameObject PotionPanel;
    public bool PotionPanelOpen = false;
    public StarterAssetsInputs StarterAssetsInputs;
    public GameObject Lab, Hall;
    public GameObject DirectionalLight;
    public OpeningCinematic openingCinematic;
    public WizardCreator wizardCreator;
    public Transform PlayerPos, AiPos;
    public GameObject[] Wizards;
    public GameObject StartScreen;
    public TextMeshProUGUI Text1, Text2, Text3;
    public GameObject WinningPanel;
    public AudioSource GameplayMusic;
    private void Awake()
    {
        _specialAttackCamera = SpecialAttackCamera.GetComponent<CinemachineVirtualCamera>();
        Perlin = BattleCamera.GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }
    private void Start()
    {
        if(PlayerPrefs.GetInt("Again")==0)
        {
            OpenMainMenu();
        }
        else
        {
            FadeInOut(0);
            Lab.SetActive(true);
            StartGame();
        }
        //StartGame();
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.SetInt("Again", 0);
    }
    public void FadeInOut(int val)
    {
        FadePanel.DOFade(val, 0.5f).SetUpdate(true);
    }

    void SetSkybox()
    {
        DirectionalLight.SetActive(true);
    }
    void RemoveSkyBox()
    {
        DirectionalLight.SetActive(false);
    }

    void OpenMainMenu()
    {
        FadeInOut(0);
        MainmenuCamera.SetActive(true);
        MainMenuPanel.SetActive(true);
        Lab.SetActive(true);
        RemoveSkyBox();
    }

    public void OpenPotionPanel()
    {
        PotionPanel.SetActive(true);
        _input.enabled = false; 
        StarterAssetsInputs.SetCursorState(false);
        StartCoroutine(Delay());
    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(0.5f);
        PotionPanelOpen = true;
    }

    public void ClosePotionPanel()
    {
        PotionPanel.SetActive(false);
        PotionPanelOpen = false;
        _input.enabled = true;
        StarterAssetsInputs.SetCursorState(true);
    }


    public void StartGame()
    {
        MainmenuCamera.SetActive(false);
        MainMenuPanel.SetActive(false);
        _input.enabled = true;
        PlayerCamera.SetActive(true);
        ClickSound.Play();
        StarterAssetsInputs.SetCursorState(true);
        wizardCreator.enabled = true;
    }

    public void QuitGame()
    {
        PlayerPrefs.SetInt("Again", 0);
        FadeInOut(1);
        StartCoroutine(Quit_Delay());
        ClickSound.Play();
    }
    IEnumerator Quit_Delay()
    {
        yield return new WaitForSeconds(0.5f);
        Application.Quit();
    }
    public void GoToCinematic()
    {
        _input.enabled = false;
        wizardCreator.enabled = false;
        FadeInOut(1);
        StartCoroutine(Del());
    }
    IEnumerator Del()
    {
        yield return new WaitForSeconds(0.5f);
        openingCinematic.CinematicCamera1.SetActive(true);
        Lab.SetActive(false);
        PlayerCamera.SetActive(false);
        _input.gameObject.SetActive(false);
        yield return new WaitForSeconds(1f);
        SetSkybox();
        FadeInOut(0);
        yield return new WaitForSeconds(0.3f);
        openingCinematic.StartCinematic();
    }


    public void StartGamePlay()
    {
        GameplayUI.ShowUI();
        player.GetComponent<PlayerInputs>().StartGame();
        enemy.GetComponent<AiControls>().StartGame();
    }

    public void SetActor(Transform transform)
    {
        _specialAttackCamera.Follow = transform;
        _specialAttackCamera.LookAt = transform;
    }
    public void Shake()
    {
        Perlin.m_AmplitudeGain = 0.5f;
        Perlin.m_FrequencyGain = 0.3f;
    }
    public void StopShake()
    {
        Perlin.m_AmplitudeGain = 0;
        Perlin.m_FrequencyGain = 0;
    }

    public void WinCinematic()
    {
        Winner._isAlive = false;
        GameplayUI.HideUI();
        if (Winner.IsPlayer)
        {
            WinCameraPlayer.SetActive(true);
        }
        else
        {
            WinCameraAI.SetActive(true);
        }
        BattleCamera.SetActive(false);
        StartCoroutine(Taunt_Delay());
    }
    IEnumerator Taunt_Delay()
    {
        yield return new WaitForSeconds(TauntDelay);
        Winner.PlayTaunt();
        yield return new WaitForSeconds(0.5f);
        WinningPanel.SetActive(true);
        StarterAssetsInputs.SetCursorState(false);
    }

    public void PlayAgain()
    {
        PlayerPrefs.SetInt("Again", 1);
        FadeInOut(1);
        StartCoroutine(Restart_Delay());
    }

    IEnumerator Restart_Delay()
    {
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(0);
    }

    [ContextMenu("StartScreen")]
    public void OpenStartScreen()
    {
        Text3.DOFade(0, 0);
        Text2.DOFade(0, 0);
        Text1.DOFade(0, 0);
        StartScreen.SetActive(true);
        StartCoroutine(CountDown());
    }

    IEnumerator CountDown()
    {
        yield return new WaitForSeconds(1f);
        Text3.DOFade(1, 0.5f);
        yield return new WaitForSeconds(1f);
        Text3.DOFade(0, 0.5f);
        yield return new WaitForSeconds(0.5f);
        Text2.DOFade(1, 0.5f);
        yield return new WaitForSeconds(1f);
        Text2.DOFade(0, 0.5f);
        yield return new WaitForSeconds(0.5f);
        Text1.DOFade(1, 0.5f);
        yield return new WaitForSeconds(1f);
        Text1.DOFade(0, 0.5f);
        yield return new WaitForSeconds(0.5f);
        StartScreen.SetActive(false);
        StartGamePlay();
    }

    IEnumerator Potion_Delay()
    {
        yield return new WaitForSeconds(0.5f);
    }

    public void PotionCheck()
    {
        switch (PotionPanelData.CurrentPotions[0].potion)
        {
            case Potion.Luck:
                switch (PotionPanelData.CurrentPotions[1].potion)
                {
                    case Potion.Sharp:
                        GameObject g = Instantiate(Wizards[0], PlayerPos.position, PlayerPos.rotation);
                        g.GetComponent<WizardStats>().WizardUI = PlayerUI;
                        player = g.GetComponent<Wizard>();
                        g.AddComponent<PlayerInputs>();
                        GameObject g1 = Instantiate(Wizards[1], AiPos.position, AiPos.rotation);
                        g1.GetComponent<WizardStats>().WizardUI = EnemyUI;
                        enemy = g1.GetComponent<Wizard>();
                        g1.AddComponent<AiControls>();
                        break;
                    case Potion.Humble:
                        GameObject g2 = Instantiate(Wizards[0], PlayerPos.position, PlayerPos.rotation);
                        g2.GetComponent<WizardStats>().WizardUI = PlayerUI;
                        player = g2.GetComponent<Wizard>();
                        g2.AddComponent<PlayerInputs>();
                        GameObject g3 = Instantiate(Wizards[1], AiPos.position, AiPos.rotation);
                        g3.GetComponent<WizardStats>().WizardUI = EnemyUI;
                        enemy = g3.GetComponent<Wizard>();
                        g3.AddComponent<AiControls>();
                        break;
                    case Potion.Cowardly:
                        GameObject g4 = Instantiate(Wizards[0], PlayerPos.position, PlayerPos.rotation);
                        g4.GetComponent<WizardStats>().WizardUI = PlayerUI;
                        player = g4.GetComponent<Wizard>();
                        g4.AddComponent<PlayerInputs>();
                        GameObject g5 = Instantiate(Wizards[1], AiPos.position, AiPos.rotation);
                        g5.GetComponent<WizardStats>().WizardUI = EnemyUI;
                        enemy = g5.GetComponent<Wizard>();
                        g5.AddComponent<AiControls>();
                        break;
                    default:
                        Debug.LogError("Error");
                        break;
                }
                break;

            case Potion.Humble:
                switch (PotionPanelData.CurrentPotions[1].potion)
                {
                    case Potion.Luck:
                        GameObject g = Instantiate(Wizards[0], PlayerPos.position, PlayerPos.rotation);
                        g.GetComponent<WizardStats>().WizardUI = PlayerUI;
                        player = g.GetComponent<Wizard>();
                        g.AddComponent<PlayerInputs>();
                        GameObject g1 = Instantiate(Wizards[1], AiPos.position, AiPos.rotation);
                        g1.GetComponent<WizardStats>().WizardUI = EnemyUI;
                        enemy = g1.GetComponent<Wizard>();
                        g1.AddComponent<AiControls>();
                        break;
                    case Potion.Sharp:
                        GameObject g2 = Instantiate(Wizards[0], PlayerPos.position, PlayerPos.rotation);
                        g2.GetComponent<WizardStats>().WizardUI = PlayerUI;
                        player = g2.GetComponent<Wizard>();
                        g2.AddComponent<PlayerInputs>();
                        GameObject g3 = Instantiate(Wizards[1], AiPos.position, AiPos.rotation);
                        g3.GetComponent<WizardStats>().WizardUI = EnemyUI;
                        enemy = g3.GetComponent<Wizard>();
                        g3.AddComponent<AiControls>();
                        break;
                    case Potion.Cowardly:
                        GameObject g4 = Instantiate(Wizards[0], PlayerPos.position, PlayerPos.rotation);
                        g4.GetComponent<WizardStats>().WizardUI = PlayerUI;
                        player = g4.GetComponent<Wizard>();
                        g4.AddComponent<PlayerInputs>();
                        GameObject g5 = Instantiate(Wizards[1], AiPos.position, AiPos.rotation);
                        g5.GetComponent<WizardStats>().WizardUI = EnemyUI;
                        enemy = g5.GetComponent<Wizard>();
                        g5.AddComponent<AiControls>();
                        break;
                    default:
                        Debug.LogError("Error");
                        break;
                }
                break;

            case Potion.Sharp:
                switch (PotionPanelData.CurrentPotions[1].potion)
                {
                    case Potion.Humble:
                        GameObject g = Instantiate(Wizards[0], PlayerPos.position, PlayerPos.rotation);
                        g.GetComponent<WizardStats>().WizardUI = PlayerUI;
                        player = g.GetComponent<Wizard>();
                        g.AddComponent<PlayerInputs>();
                        GameObject g1 = Instantiate(Wizards[1], AiPos.position, AiPos.rotation);
                        g1.GetComponent<WizardStats>().WizardUI = EnemyUI;
                        enemy = g1.GetComponent<Wizard>();
                        g1.AddComponent<AiControls>();
                        break;
                    case Potion.Luck:
                        GameObject g2 = Instantiate(Wizards[0], PlayerPos.position, PlayerPos.rotation);
                        g2.GetComponent<WizardStats>().WizardUI = PlayerUI;
                        player = g2.GetComponent<Wizard>();
                        g2.AddComponent<PlayerInputs>();
                        GameObject g3 = Instantiate(Wizards[1], AiPos.position, AiPos.rotation);
                        g3.GetComponent<WizardStats>().WizardUI = EnemyUI;
                        enemy = g3.GetComponent<Wizard>();
                        g3.AddComponent<AiControls>();
                        break;
                    case Potion.Cowardly:
                        GameObject g4 = Instantiate(Wizards[0], PlayerPos.position, PlayerPos.rotation);
                        g4.GetComponent<WizardStats>().WizardUI = PlayerUI;
                        player = g4.GetComponent<Wizard>();
                        g4.AddComponent<PlayerInputs>();
                        GameObject g5 = Instantiate(Wizards[1], AiPos.position, AiPos.rotation);
                        g5.GetComponent<WizardStats>().WizardUI = EnemyUI;
                        enemy = g5.GetComponent<Wizard>();
                        g5.AddComponent<AiControls>();
                        break;
                    default:
                        Debug.LogError("Error");
                        break;
                }
                break;
            case Potion.Cowardly:
                switch (PotionPanelData.CurrentPotions[1].potion)
                {
                    case Potion.Humble:
                        GameObject g = Instantiate(Wizards[0], PlayerPos.position, PlayerPos.rotation);
                        g.GetComponent<WizardStats>().WizardUI = PlayerUI;
                        player = g.GetComponent<Wizard>();
                        g.AddComponent<PlayerInputs>();
                        GameObject g1 = Instantiate(Wizards[1], AiPos.position, AiPos.rotation);
                        g1.GetComponent<WizardStats>().WizardUI = EnemyUI;
                        enemy = g1.GetComponent<Wizard>();
                        g1.AddComponent<AiControls>();
                        break;
                    case Potion.Luck:
                        GameObject g2 = Instantiate(Wizards[0], PlayerPos.position, PlayerPos.rotation);
                        g2.GetComponent<WizardStats>().WizardUI = PlayerUI;
                        player = g2.GetComponent<Wizard>();
                        g2.AddComponent<PlayerInputs>();
                        GameObject g3 = Instantiate(Wizards[1], AiPos.position, AiPos.rotation);
                        g3.GetComponent<WizardStats>().WizardUI = EnemyUI;
                        enemy = g3.GetComponent<Wizard>();
                        g3.AddComponent<AiControls>();
                        break;
                    case Potion.Sharp:
                        GameObject g4 = Instantiate(Wizards[0], PlayerPos.position, PlayerPos.rotation);
                        g4.GetComponent<WizardStats>().WizardUI = PlayerUI;
                        player = g4.GetComponent<Wizard>();
                        g4.AddComponent<PlayerInputs>();
                        GameObject g5 = Instantiate(Wizards[1], AiPos.position, AiPos.rotation);
                        g5.GetComponent<WizardStats>().WizardUI = EnemyUI;
                        enemy = g5.GetComponent<Wizard>();
                        g5.AddComponent<AiControls>();
                        break;
                    default:
                        Debug.LogError("Error");
                        break;
                }
                break;
            default:
                Debug.LogError("Invalid object to instantiate. Check the input.");
                break;
        }
    }
}
