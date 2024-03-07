using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragAndDrop : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Vector3 originalPosition;
    Canvas canvas;
    public PotionSlot dropTarget;
    public bool CanDrag = true;
    public TextMeshProUGUI tooltipText; // Reference to the Text component for displaying tooltips
    public Potion potion;
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponentInParent<CanvasGroup>();
        canvas = GetComponentInParent<Canvas>();
        originalPosition = rectTransform.anchoredPosition;
        tooltipText.text = potion.ToString();
        CanDrag = true;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(CanDrag)
        {
            tooltipText.gameObject.SetActive(true);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Hide tooltip text when not hovering
        tooltipText.gameObject.SetActive(false);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (CanDrag)
        {
            //canvasGroup.blocksRaycasts = false;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (CanDrag)
        {
            rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if(CanDrag)
        {
            canvasGroup.blocksRaycasts = true;
            if (dropTarget != null)
            {
                CanDrag = false;
            }
            else
            {
                CanDrag = false;
                rectTransform.DOAnchorPos(originalPosition, 0.5f).OnComplete(() => 
                {
                    CanDrag = true;
                });
            }
        }
    }
    public void Reset()
    {
        rectTransform.anchoredPosition = originalPosition;
        CanDrag = true;
        dropTarget = null;
    }

    public enum Potion
    {
        Luck,
        Sharp,
        Humble,
        Cowardly,
        Wise,
        Caring,
        Selfish,
        Narcissism
    }
}
