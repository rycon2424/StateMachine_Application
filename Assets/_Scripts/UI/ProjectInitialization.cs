using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProjectInitialization : MonoBehaviour
{
    public InputField[] inputFields;
    public Button startProject;

    public void SaveSettingsToAllInfo()
    {
        AllInfo.instance.projectName = inputFields[0].text;
        AllInfo.instance.stateMachineName = inputFields[1].text;
        AllInfo.instance.mainClass = inputFields[2].text;
        AllInfo.instance.onEnterFunctionName = inputFields[3].text;
        AllInfo.instance.onExitFunctionName = inputFields[4].text;
        AllInfo.instance.onUpdateFunctionName = inputFields[5].text;
    }

    void Update()
    {
        bool buttonInteractive = true;
        foreach (var ipf in inputFields)
        {
            if (ipf.text.Length < 4)
            {
                buttonInteractive = false;
            }
        }
        startProject.interactable = buttonInteractive;
    }
}
