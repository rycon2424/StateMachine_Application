using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public class Inspector : MonoBehaviour
{
    public static Inspector instance;

    public GameObject objectSelected;
    [ReadOnly] [ShowInInspector] GameObject blockComponent;
    [Space]
    public Image bgImage;
    public InputField stateNameField;

    [Header("Color Component")]
    public GameObject currentBlock;
    public Slider sliderRed;
    public Slider sliderGreen;
    public Slider sliderBlue;

    [Header("Connections Component")]
    public GameObject conditionPrefab;
    public Transform conditionsContentBox;
    public Vector3 startSpawnPos;
    private List<GameObject> conditionList = new List<GameObject>();

    [Header("Conditions Component")]
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
        if (currentBlock != null)
        {
            ClearInspector();
        }

        currentBlock = g;
        Block blockComponent = g.GetComponent<Block>();

        ogName = blockComponent.blockName;
        stateNameField.text = ogName;

        sliderRed.value = c.r;
        sliderGreen.value = c.g;
        sliderBlue.value = c.b;

        for (int i = 0; i < blockComponent.connections.Count; i++)
        {
            GameObject go = Instantiate(conditionPrefab, conditionsContentBox);
            Vector3 spawnPos = startSpawnPos - new Vector3(0, 20 * i, 0);
            go.GetComponent<RectTransform>().position = spawnPos;
            string connectionText = blockComponent.connections[i].from.block.blockName + " > " + blockComponent.connections[i].to.block.blockName;
            go.GetComponent<ConnectionBox>().transitionText.text = connectionText;
            conditionList.Add(go);
        }

        objectSelected.SetActive(true);
    }

    void ClearInspector()
    {
        foreach (var condition in conditionList)
        {
            Destroy(condition);
        }
        conditionList.Clear();
    }

    public void ColorUpdate()
    {
        Color c = new Color( sliderRed.value, sliderGreen.value,  sliderBlue.value );
        currentBlock.GetComponent<Image>().color = c;
        currentBlock.GetComponent<Block>().imagecolor = c;
        bgImage.color = c;
    }

    public void RemoveConnection(Node nodeToRemove)
    {

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
