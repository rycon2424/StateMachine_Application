using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InfoDisplay : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject hintCloud;
    public Text hintTextObject;
    [TextArea]
    public string hintText;

    void Awake()
    {
        hintTextObject.text = hintText;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        hintCloud.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        hintCloud.SetActive(false);
    }

        
}
