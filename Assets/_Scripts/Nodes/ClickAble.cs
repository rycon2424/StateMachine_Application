using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ClickAble : MonoBehaviour, IPointerClickHandler
{

    private Image colorImage;
    void Awake()
    {
        colorImage = GetComponent<Image>();
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        Inspector.instance.LoadInspector(gameObject, colorImage.color);
    }
}
