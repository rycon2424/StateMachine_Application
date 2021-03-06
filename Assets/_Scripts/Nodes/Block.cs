using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public class Block : MonoBehaviour
{
    public string blockName;
    public bool enterState;
    [SerializeField] Text blockNameText;
    public Color imagecolor;
    [SerializeField] GameObject enterStateIndicator;
    [Space]
    public List<Node> connections;
    [ReadOnly] public int id;

    public void CreateNewBlock()
    {
        Color randomC = new Color(Random.Range(0.1f, 1f), Random.Range(0.1f, 1f), Random.Range(0.1f, 1f));
        Image i = GetComponent<Image>();
        i.color = randomC;
        imagecolor = i.color;
        id = AllInfo.instance.GetID();
        enterState = AllInfo.instance.IsOnlyBlock();
        UpdateEnterState(enterState);
        AllInfo.instance.blocks.Add(this);
    }

    public void LoadBlock()
    {
        Image i = GetComponent<Image>();
        i.color = imagecolor;
        AllInfo.instance.blocks.Add(this);
    }

    public void SetName(string bName)
    {
        blockName = bName;
        blockNameText.text = blockName; 
    }

    public void UpdateEnterState(bool isEnterState)
    {
        enterState = isEnterState;
        enterStateIndicator.SetActive(isEnterState);
    }
}
