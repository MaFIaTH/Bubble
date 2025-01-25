using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class OnHover : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
    

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.DOScale(1.2f,0.5f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.DOScale(1f,0.5f);
    }
}
