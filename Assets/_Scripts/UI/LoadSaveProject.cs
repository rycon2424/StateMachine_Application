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
        sf.blocks = new List<BlockInfo>();
        foreach (var block in AllInfo.instance.blocks)
        {
            BlockInfo blockToSave = new BlockInfo();

            blockToSave.blockName = block.blockName;
            blockToSave.blockPosition = block.GetComponent<RectTransform>().position;
            blockToSave.color = new Vector3(block.imagecolor.r, block.imagecolor.g, block.imagecolor.b);
            blockToSave.enterState = block.enterState;
            blockToSave.id = block.id;

            sf.blocks.Add(blockToSave);
        }

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
        foreach (BlockInfo block in sf.blocks)
        {
            applicationController.SpawnLoadedBlock(block);
        }
    }
}

[System.Serializable]
public class SaveFile
{
    public string projectName;
    public List<BlockInfo> blocks = new List<BlockInfo>();
}

[System.Serializable]
public class BlockInfo
{
    public string blockName;
    public Vector3 blockPosition;
    public Vector3 color;
    public bool enterState;
    public int id;
}