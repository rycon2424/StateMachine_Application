using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System.IO;
using UnityEngine.Assertions;
using System.Runtime.Serialization.Formatters.Binary;

public class AllInfo : MonoBehaviour
{
    [ReadOnly] public string projectName;
    [ReadOnly] public string stateMachineName;
    [ReadOnly] public string mainClass;
    [ReadOnly] public string onEnterFunctionName;
    [ReadOnly] public string onExitFunctionName;
    [ReadOnly] public string onUpdateFunctionName;

    private List<Block> blocks = new List<Block>();
    [SerializeField] List<int> takenIDs = new List<int>();

    public static AllInfo instance;

    BinaryFormatter bFormatter;

    private void Awake()
    {
        if (instance != null)
            Destroy(instance);
        bFormatter = new BinaryFormatter();
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

    [Button]
    public void Export()
    {
        if (PlayerPrefs.HasKey("Build") == false)
            PlayerPrefs.SetInt("Build", 0);
        else
            PlayerPrefs.SetInt("Build", (PlayerPrefs.GetInt("Build") + 1));

        string pathForExport = Application.persistentDataPath + $"/Builds({PlayerPrefs.GetInt("Build")})";
        Directory.CreateDirectory(pathForExport);

        CreateMainClass(pathForExport);
    }

    void CreateMainClass(string path)
    {
        string mainClassScript = path + $"/{mainClass}.cs";
        if (!File.Exists(mainClassScript))
        {
            File.WriteAllText(mainClassScript, "lol");
            Debug.Log("Created mainclass inside " + path);
        }
    }

    void CreateStateMachine(string path)
    {

    }

    void CreateState(string path, List<Conditions> cons)
    {

    }

    public SaveFile sf;

    void SaveProject(string path)
    {
        string url = Path.Combine(path, projectName);

        FileStream fstream = null;

        try
        {
            fstream = new FileStream(path, FileMode.Create);
            sf = new SaveFile();

            bFormatter.Serialize(fstream, sf);

            fstream.Flush();
            fstream.Close();
        }
        catch (System.Exception error)
        {
            Debug.Log(error.Message);
            throw;
        }
    }

}

[System.Serializable]
public class SaveFile
{

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

