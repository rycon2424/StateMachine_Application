using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System.IO;
using UnityEngine.SceneManagement;

public class LoadSaveProject : MonoBehaviour
{
    [SerializeField] ApplicationControl applicationController;
    [SerializeField] LoadFile fileLoader;

    [Button]
    public void SaveProjectTo()
    {
        string saveFilePath = Application.persistentDataPath + "/Projects";
        Directory.CreateDirectory(saveFilePath);
        saveFilePath += "/" + AllInfo.instance.projectName + ".json"; 
        SaveProject(saveFilePath);
    }

    [Button]
    public void LoadProjectFrom()
    {
        string fileName = fileLoader.fileSelect.options[fileLoader.fileSelect.value].text;
        string loadfilePath = Application.persistentDataPath + "/Projects";
        Directory.CreateDirectory(loadfilePath);
        loadfilePath += "/" + fileName;
        LoadProject(loadfilePath);
    }

    void SaveProject(string path)
    {
        SaveFile sf = new SaveFile();

        sf.projectName = AllInfo.instance.projectName;

        SaveSettings(sf);
        SaveBlocks(sf);

        string savefile = JsonUtility.ToJson(sf);
        try
        {
            File.WriteAllText(path, savefile);
            ApplicationConsole.console.UpdateConsole("Saved to " + path);
            Debug.Log("Saved to " + path);
        }
        catch (System.Exception e)
        {
            ApplicationConsole.console.UpdateConsole(e.Message);
        }
    }

    void SaveBlocks(SaveFile sf)
    {
        foreach (var block in AllInfo.instance.blocks)
        {
            BlockInfo blockToSave = new BlockInfo();

            blockToSave.blockName = block.blockName;
            blockToSave.blockPosition = block.GetComponent<RectTransform>().position;
            blockToSave.color = new Vector3(block.imagecolor.r, block.imagecolor.g, block.imagecolor.b);
            blockToSave.enterState = block.enterState;
            blockToSave.id = block.id;
            blockToSave.allConnections = new List<Connection>();

            foreach (Node con in block.connections)
            {
                Connection newConnection = new Connection();
                newConnection.from = con.from.block.blockName;
                newConnection.to = con.to.block.blockName;
                newConnection.conditions = new List<Conditions>();

                foreach (Conditions condition in con.cons)
                {
                    newConnection.conditions.Add(condition);
                }

                blockToSave.allConnections.Add(newConnection);
            }

            sf.blocks.Add(blockToSave);
        }
    }

    void SaveSettings(SaveFile sf)
    {
        sf.projectInfo.stateMachineName = AllInfo.instance.stateMachineName;
        sf.projectInfo.mainClass = AllInfo.instance.mainClass;
        sf.projectInfo.onEnterFunctionName = AllInfo.instance.onEnterFunctionName;
        sf.projectInfo.onExitFunctionName = AllInfo.instance.onExitFunctionName;
        sf.projectInfo.onUpdateFunctionName = AllInfo.instance.onUpdateFunctionName;
    }

    void LoadProject(string path)
    {
        string projectToString = "";
        SaveFile sf = new SaveFile();
        try
        {
            projectToString = File.ReadAllText(path);
            sf = JsonUtility.FromJson<SaveFile>(projectToString);
            LoadUpProject(sf);
            ApplicationConsole.console.UpdateConsole("Succesfully loaded project: " + sf.projectName);
        }
        catch (System.Exception e)
        {
            ApplicationConsole.console.UpdateConsole(e.Message);
        }
    }

    public void LoadUpProject(SaveFile sf)
    {
        AllInfo.instance.projectName = sf.projectName;
        AllInfo.instance.mainClass = sf.projectInfo.mainClass;
        AllInfo.instance.stateMachineName = sf.projectInfo.stateMachineName;
        AllInfo.instance.onUpdateFunctionName = sf.projectInfo.onUpdateFunctionName;
        AllInfo.instance.onEnterFunctionName = sf.projectInfo.onEnterFunctionName;
        AllInfo.instance.onExitFunctionName = sf.projectInfo.onExitFunctionName;

        foreach (BlockInfo block in sf.blocks)
        {
            applicationController.SpawnLoadedBlock(block);
        }

        foreach (BlockInfo block in sf.blocks)
        {
            foreach (Connection connection in block.allConnections)
            {
                CreateConnection(connection.from, connection.to, connection.conditions);
            }
        }
    }

    void CreateConnection(string from, string to, List<Conditions> conditions)
    {
        Block fromBlock = null;
        Block toBlock = null;
        // Get From Block
        foreach (Block createdBlock in AllInfo.instance.blocks)
        {
            if (createdBlock.blockName == from)
            {
                fromBlock = createdBlock;
                break;
            }
        }
        // Get To Block
        foreach (Block createdBlock in AllInfo.instance.blocks)
        {
            if (createdBlock.blockName == to)
            {
                toBlock = createdBlock;
                break;
            }
        }
        NodeAble nodeRef = fromBlock.GetComponent<NodeAble>();
        if (fromBlock != null && toBlock != null)
        {
            nodeRef.CreateConnection(toBlock.GetComponent<NodeAble>(), null, conditions);
        }
        else
        {
            Debug.LogError("From or To block does not exist in allinfo.instance.blocks");
        }
    }
}

[System.Serializable]
public class SaveFile
{
    public string projectName;
    public ProjectNames projectInfo = new ProjectNames();
    public List<BlockInfo> blocks = new List<BlockInfo>();
}

[System.Serializable]
public class ProjectNames
{
    public string stateMachineName;
    public string mainClass;
    public string onEnterFunctionName;
    public string onExitFunctionName;
    public string onUpdateFunctionName;
}


[System.Serializable]
public class BlockInfo
{
    public string blockName;
    public Vector3 blockPosition;
    public Vector3 color;
    public bool enterState;
    public int id;
    public List<Connection> allConnections;
}

[System.Serializable]
public class Connection
{
    public string from;
    public string to;
    public List<Conditions> conditions;
}