using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class LoadSaveProject : MonoBehaviour
{
    public SaveFile sf;

    BinaryFormatter bFormatter;

    private void Awake()
    {
        bFormatter = new BinaryFormatter();
    }

    void SaveProject(string path)
    {
        string url = Path.Combine(path, AllInfo.instance.projectName);

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