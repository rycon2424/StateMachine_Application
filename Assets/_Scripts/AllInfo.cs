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

    public int GetID()
    {
        int randomID = Random.Range(0, 50000);
        while (takenIDs.Contains(randomID))
        {
            randomID = Random.Range(0, 50000);
        }
        takenIDs.Add(randomID);
        return randomID;
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
public class Connection
{
    public string from;
    public string to;
    public Conditions[] conditions;
    // 0 = boolean == / =!
    // 1 = int > / < / =
    // 2 = float > / < / ==
}

[System.Serializable]
public class Conditions
{
    public string conditionName = "";
    public int typeCondition = 0;
    // 0 = boolean == / =!
    // 1 = int > / < / == / <= / >=
    // 2 = float > / < / == / <= / >=
    public bool boolValue = true;
    public string intValue = "0";
    public string floatValue = "0";

    public int intFloatCon;
    // 0 ==
    // 1 >
    // 2 <
    // 3 <=
    // 4 >=
    public int id;
}

