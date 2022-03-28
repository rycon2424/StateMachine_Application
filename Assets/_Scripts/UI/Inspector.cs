using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public class Inspector : MonoBehaviour
{
    public static Inspector instance;

    public GameObject objectSelected;
    [ReadOnly] [ShowInInspector] Block blockComponent;
    [Space]
    public Image bgImage;
    public InputField stateNameField;
    public Toggle deleteToggle;
    public Button deleteButton;

    [Header("Color Component")]
    [ReadOnly] public GameObject currentBlock;
    public Slider sliderRed;
    public Slider sliderGreen;
    public Slider sliderBlue;

    [Header("Connections Component")]
    public RectTransform contentBox;
    public GameObject conditionPrefab;
    public Transform conditionsContentBox;
    public Vector3 startSpawnPos;
    public int spacing = 30;
    public int spacingAfterNewCon;

    private Color cleanColor;
    private List<GameObject> conditionList = new List<GameObject>();

    ApplicationControl ac;

    private void Awake()
    {
        objectSelected.SetActive(false);
        ac = FindObjectOfType<ApplicationControl>();

        if (instance)
            Destroy(instance);

        instance = this;
    }

    private void Start()
    {
        cleanColor = bgImage.color;
    }

    public void ReloadConditions()
    {
        ClearInspector();
        ConditionComponent();
    }

    public void CleanInspector()
    {
        bgImage.color = cleanColor;
        objectSelected.SetActive(false);
    }

    public void LoadInspector(GameObject g, Color c)
    {
        if (currentBlock != null)
        {
            ClearInspector();
        }

        currentBlock = g;
        blockComponent = g.GetComponent<Block>();

        ogName = blockComponent.blockName;
        stateNameField.text = ogName;

        sliderRed.value = c.r;
        sliderGreen.value = c.g;
        sliderBlue.value = c.b;

        ConditionComponent();

        objectSelected.SetActive(true);
    }

    void ClearInspector()
    {
        foreach (var condition in conditionList)
        {
            Destroy(condition);
        }
        conditionList.Clear();
        deleteToggle.isOn = false;
        deleteButton.interactable = false;
    }

    void ConditionComponent()
    {
        int totalCond = 0;
        int sizeMultiplyer = 0;

        for (int i = 0; i < blockComponent.connections.Count; i++)
        {
            sizeMultiplyer += blockComponent.connections[i].cons.Count;
        }

        contentBox.sizeDelta = new Vector2(contentBox.sizeDelta.x, 500 + 100 * (blockComponent.connections.Count + sizeMultiplyer));
        for (int i = 0; i < blockComponent.connections.Count; i++)
        {
            GameObject go = Instantiate(conditionPrefab, conditionsContentBox);
            
            Vector3 spawnPos = startSpawnPos - new Vector3(0, spacing * i + (spacingAfterNewCon * totalCond), 0);
            go.GetComponent<RectTransform>().anchoredPosition = spawnPos;

            string connectionText = blockComponent.connections[i].from.block.blockName + " > " + blockComponent.connections[i].to.block.blockName;

            ConnectionBox conBox = go.GetComponent<ConnectionBox>();
            conBox.transitionText.text = connectionText;
            conBox.thisNode = blockComponent.connections[i];

            Node temp = blockComponent.connections[i];
            conBox.delete.onClick.AddListener(() => RemoveConnection(temp));
            conditionList.Add(go);

            for (int p = 0; p < conBox.thisNode.cons.Count; p++)
            {
                conBox.LoadCondition(conBox.thisNode.cons[p], p);
                totalCond++;
            }
        }
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
        blockComponent.connections.Remove(nodeToRemove);
        Destroy(nodeToRemove.gameObject);
        LoadInspector(currentBlock, currentBlock.GetComponent<Image>().color);
    }

    [ShowInInspector] [ReadOnly] private string ogName;
    public void ChangeStateName()
    {
        bool existsAlready = false;
        foreach (var state in AllInfo.instance.blocks)
        {
            Debug.Log($"{state.blockName} == {stateNameField.text}");
            if (state.blockName == stateNameField.text)
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
            ogName = stateNameField.text;
            currentBlock.GetComponent<Block>().SetName(ogName);
        }
    }

    public void RemoveBlock()
    {
        AllInfo.instance.RemoveBlock(blockComponent);
        AllInfo.instance.RemoveID(blockComponent.id);
        Destroy(currentBlock);
        ClearInspector();
        CleanInspector();
    }

    public void SwitchEnterState()
    {
        foreach (var block in AllInfo.instance.blocks)
        {
            if (block.enterState == true)
            {
                block.UpdateEnterState(false);
                break;
            }
        }
        blockComponent.UpdateEnterState(true);
    }

}