using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using TMPro;

public class LoadFile : MonoBehaviour
{
    public TMP_Dropdown fileSelect;
    [SerializeField] Button loadProject;

    void Start()
    {
        ReloadBrowser();
    }

    public void ReloadBrowser()
    {
        string saveFilePath = Application.persistentDataPath + "/Projects";
        Directory.CreateDirectory(saveFilePath);
        DirectoryInfo directoryInfo = new DirectoryInfo(saveFilePath);
        FileInfo[] fileInfo = directoryInfo.GetFiles("*.*", SearchOption.AllDirectories);
        fileSelect.options.Clear();
        foreach (FileInfo file in fileInfo)
        {
            TMP_Dropdown.OptionData optionData = new TMP_Dropdown.OptionData(file.Name);
            fileSelect.options.Add(optionData);
            fileSelect.value = 1;
        }
        if (fileInfo.Length < 1)
        {
            loadProject.interactable = false;
        }
    }
}
