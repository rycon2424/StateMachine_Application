using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Block : MonoBehaviour
{
    public string blockName;
    [SerializeField] Text blockNameText;
    public Color imagecolor;
    [Space]
    public List<Node> connections;

    void Awake()
    {
        imagecolor = GetComponent<Image>().color;
    }

    public void SetName(string bName)
    {
        blockName = bName;
        blockNameText.text = blockName; 
    }
}
