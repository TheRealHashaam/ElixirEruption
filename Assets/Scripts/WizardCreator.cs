using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WizardCreator : MonoBehaviour
{
    public Camera playerCamera;
    public float interactionDistance;
    public TextMeshProUGUI textMeshProUGUI;
    public Vector3 Offset;
    GameManager _gameManager;

    private void Start()
    {
        _gameManager = FindObjectOfType<GameManager>();
    }

    void Update()
    {
        RaycastHit hit;
        Ray ray = new Ray(playerCamera.transform.position + Offset, playerCamera.transform.forward);
        if (Physics.Raycast(ray, out hit, interactionDistance))
        {
            Pot pot = hit.collider.gameObject.GetComponent<Pot>();
            if (pot)
            {
                textMeshProUGUI.text = "Press E to interact";
                if (!_gameManager.PotionPanelOpen && Input.GetKeyUp(KeyCode.E))
                {
                    _gameManager.OpenPotionPanel();
                }
            }
            else
            {
                textMeshProUGUI.text = "";
            }
        }
        else
        {
            textMeshProUGUI.text = "";
        }
        
        if (_gameManager.PotionPanelOpen)
        {
            textMeshProUGUI.text = "";
            if (_gameManager.PotionPanelOpen && Input.GetKeyUp(KeyCode.E))
            {
                _gameManager.ClosePotionPanel();
            }
        }
        //Debug.DrawRay(ray.origin, ray.direction * interactionDistance, Color.green);
    }
    private void OnDisable()
    {
        textMeshProUGUI.text = "";
    }
}
