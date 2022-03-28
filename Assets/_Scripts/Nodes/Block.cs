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
    [Space]
    public List<Node> connections;
    [ReadOnly] public int id;

    void Awake()
    {
        Color randomC = new Color(Random.Range(0.1f, 1f), Random.Range(0.1f, 1f), Random.Range(0.1f, 1f));
        Image i = GetComponent<Image>();
        i.color = randomC;
        imagecolor = i.color;
        id = AllInfo.instance.GetID();
        AllInfo.instance.blocks.Add(this);
    }

    public void SetName(string bName)
    {
        blockName = bName;
        blockNameText.text = blockName; 
    }
}
