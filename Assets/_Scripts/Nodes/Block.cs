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
        Color randomC = new Color(Random.Range(0.1f, 1f), Random.Range(0.1f, 1f), Random.Range(0.1f, 1f));
        Image i = GetComponent<Image>();
        i.color = randomC;
        imagecolor = i.color;
    }

    public void SetName(string bName)
    {
        blockName = bName;
        blockNameText.text = blockName; 
    }
}
