using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inspector : MonoBehaviour
{
    public static Inspector instance;

    public GameObject objectSelected;
    public Image bgImage;
    public InputField stateNameField;

    [Header("Color Component")]
    public GameObject currentBlock;
    public Slider sliderRed;
    public Slider sliderGreen;
    public Slider sliderBlue;

    [Header("Conditions Component")]
    public GameObject conditionPrefab;

    [Header("Connections Component")]
    public GameObject connectionPrefab;

    ApplicationControl ac;

    private void Awake()
    {
        objectSelected.SetActive(false);
        ac = FindObjectOfType<ApplicationControl>();

        if (instance)
            Destroy(instance);

        instance = this;
    }

    public void LoadInspector(GameObject g, Color c)
    {
        currentBlock = g;
        ogName = g.GetComponent<Block>().blockName;
        stateNameField.text = ogName;
        objectSelected.SetActive(true);
        sliderRed.value = c.r;
        sliderGreen.value = c.g;
        sliderBlue.value = c.b;
    }

    public void ColorUpdate()
    {
        Color c = new Color( sliderRed.value, sliderGreen.value,  sliderBlue.value );
        currentBlock.GetComponent<Image>().color = c;
        currentBlock.GetComponent<Block>().imagecolor = c;
        bgImage.color = c;
    }

    private string ogName;
    public void ChangeStateName()
    {
        bool existsAlready = false;
        foreach (var state in ac.existingStates)
        {
            if (state == stateNameField.text)
            {
                existsAlready = true;
            }
        }
        if (stateNameField.text.Length < 1)
        {
            existsAlready = true;
        }
        if (existsAlready)
        {
            stateNameField.text = ogName;
        }
        else
        {
            ac.existingStates.Remove(ogName);
            ogName = stateNameField.text;
            currentBlock.GetComponent<Block>().SetName(ogName);
            ac.existingStates.Add(ogName);
        }
    }

}
