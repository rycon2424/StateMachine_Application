using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inspector : MonoBehaviour
{
    public static Inspector instance;

    public GameObject objectSelected;
    public Image bgImage;

    [Header("Color Component")]
    public GameObject currentBlock;
    public Slider sliderRed;
    public Slider sliderGreen;
    public Slider sliderBlue;

    [Header("Conditions Component")]
    public GameObject conditionPrefab;

    [Header("Connections Component")]
    public GameObject connectionPrefab;

    private void Awake()
    {
        objectSelected.SetActive(false);
        if (instance)
            Destroy(instance);
        instance = this;
    }

    public void LoadInspector(GameObject g, Color c)
    {
        currentBlock = g;
        objectSelected.SetActive(true);
        sliderRed.value = c.r;
        sliderGreen.value = c.g;
        sliderBlue.value = c.b;
    }

    public void ColorUpdate()
    {
        Color c = new Color( sliderRed.value, sliderBlue.value,  sliderGreen.value );
        currentBlock.GetComponent<Image>().color = c;
        currentBlock.GetComponent<NodeAble>().imagecolor = c;
        bgImage.color = c;
    }

}
