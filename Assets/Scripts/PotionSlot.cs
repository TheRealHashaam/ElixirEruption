using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PotionSlot : MonoBehaviour
{
    public Color NormalColor, PotionColor;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<DragAndDrop>())
        {
            this.GetComponent<BoxCollider2D>().enabled = false;
            DragAndDrop dragAndDrop = collision.GetComponent<DragAndDrop>();
            dragAndDrop.dropTarget = this;
            dragAndDrop.CanDrag = false;
            SetPosition(dragAndDrop.GetComponent<RectTransform>(), dragAndDrop);

        }
    }

    void SetPosition(RectTransform rect,DragAndDrop d)
    {
        Vector2 thisRectAnchoredPosition;
        thisRectAnchoredPosition = this.GetComponent<RectTransform>().anchoredPosition;
        rect.DOAnchorPos(thisRectAnchoredPosition, 0.2f);
        this.GetComponent<Image>().color = PotionColor;
        PotionPanelData potionPanelData = GetComponentInParent<PotionPanelData>();
        potionPanelData.AddPotion(d);
    }
    public void Reset()
    {
        this.GetComponent<Image>().color = NormalColor;
        this.GetComponent<BoxCollider2D>().enabled = true;
    }


}
