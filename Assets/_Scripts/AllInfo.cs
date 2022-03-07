using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllInfo : MonoBehaviour
{
    private List<Block> blocks = new List<Block>();
    [SerializeField] List<int> takenIDs = new List<int>();

    public static AllInfo instance;

    private void Awake()
    {
        if (instance != null)
            Destroy(instance);
        instance = this;
    }

    public void AddNewBlock(Block blockToAdd)
    {

    }

    public void RemoveBlock(Block blockToRemove)
    {

    }


    public void GetBlock()
    {

    }

    public void Save()
    {
        
    }

}

[System.Serializable]
class BlockInfo
{
    public Vector3 position;
    public Color blockColor;
    public string blockName;
}

[System.Serializable]
class Connection
{
    public string from;
    public string to;
    public int condition;
}
