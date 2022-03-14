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

    public List<Block> blocks = new List<Block>();
    public List<string> globalVariables = new List<string>();
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

    public void RemoveID(int id)
    {
        takenIDs.Remove(id);
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
        CreateDefaultState(pathForExport);
        CreateStateMachine(pathForExport);
        foreach (var stateClass in blocks)
        {
            CreateState(pathForExport, stateClass);
        }
        Debug.Log("Exported to " + pathForExport);
    }

    void CreateMainClass(string path)
    {
        string mainClassScript = path + $"/{mainClass}.cs";
        if (!File.Exists(mainClassScript))
        {
            File.WriteAllText(mainClassScript, "lol");
        }
    }

    void CreateDefaultState(string path)
    {
        string mainClassScript = path + $"/{stateMachineName}.cs";
        if (!File.Exists(mainClassScript))
        {
            string scriptContent = "";

            scriptContent += "using System.Collections;@using System.Collections.Generic;@using UnityEngine;";
            scriptContent += "@";
            scriptContent += "@public class State@{";
            scriptContent += "@public virtual void " + onEnterFunctionName + "(" + mainClass + " actor) @{@}@";
            scriptContent += "@public virtual void " + onExitFunctionName + "(" + mainClass + " actor) @{@}@";
            scriptContent += "@public virtual void " + onUpdateFunctionName + "(" + mainClass + " actor) @{@}@";
            scriptContent += "@}";

            scriptContent = scriptContent.Replace("@", System.Environment.NewLine);
            File.WriteAllText(mainClassScript, scriptContent);
        }
    }

    void CreateStateMachine(string path)
    {
        string mainClassScript = path + $"/{stateMachineName}.cs";
        if (!File.Exists(mainClassScript))
        {
            string scriptContent = "";

            scriptContent += "using System.Collections;@using System.Collections.Generic;@using UnityEngine;";
            scriptContent += "@@public class " + stateMachineName + "@{";
            scriptContent += "@private State currentState";
            scriptContent += "@private List<State> allStates = new List<State>();";
            scriptContent += "@bool stateExists;";

            //GoToState Function
            scriptContent += "@@ public void GoToState(" + mainClass + " actor, string newstate)";
            scriptContent += "@if (LockStateMachine)@{@return;@ }";
            scriptContent += "@stateExists = false;";
            scriptContent += "@foreach (var s in allStates)@{@if (s.GetType().ToString() == newstate)@{@stateExists = true;@break;@}@}";
            scriptContent += "@if (stateExists == false)@{@return;@}";
            scriptContent += "@if (currentState != null)@{@ currentState." + onExitFunctionName +"(actor); @}";
            scriptContent += "@foreach (var s in allStates)@{@if (s.GetType().ToString() == newstate)@{";
            scriptContent += "@currentState = s;@@actor.ChangeState(currentState);@@ s." + onEnterFunctionName + "(actor);@return;@}@}@}";

            // GetSet Lock Machine
            scriptContent += "@@public bool LockStateMachine";
            scriptContent += "@{";
            scriptContent += "@get;";
            scriptContent += "@set";
            scriptContent += "@}";

            // AddState
            scriptContent += "@@public void AddState(State newState)";
            scriptContent += "@{";
            scriptContent += "@allStates.Add(newState);";
            scriptContent += "@}";

            // CurrentState
            scriptContent += "@@public State CurrentState()";
            scriptContent += "@{";
            scriptContent += "@return currentState;";
            scriptContent += "@}";

            // IsInState
            scriptContent += "@@public bool IsInState(string state)";
            scriptContent += "@{";
            scriptContent += "@ if (state == currentState.GetType().ToString())";
            scriptContent += "@{";
            scriptContent += "@return true;";
            scriptContent += "@}";
            scriptContent += "@return false;";
            scriptContent += "@}";
            scriptContent += "@}";

            scriptContent = scriptContent.Replace("@", System.Environment.NewLine);
            File.WriteAllText(mainClassScript, scriptContent);
        }
    }

    void CreateState(string path, Block className)
    {
        string mainClassScript = path + $"/{className.blockName}.cs";
        if (!File.Exists(mainClassScript))
        {
            string scriptContent = "";

            // Variables
            foreach (Node node in className.connections)
            {
                foreach (Conditions condition in node.cons)
                {
                    scriptContent += Variable(condition);
                    scriptContent += "@";
                }
            }

            // Conditions
            foreach (Node node in className.connections)
            {
                for (int i = 0; i < node.cons.Count; i++)
                {
                    scriptContent += Condition(node.cons[i]);
                    if (i + 1 < node.cons.Count)
                    {
                        scriptContent += " && ";
                    }
                }
            }

            scriptContent = scriptContent.Replace("@", System.Environment.NewLine);

            File.WriteAllText(mainClassScript, scriptContent);
        }
    }

    string Variable(Conditions con)
    {
        string condition = "";

        switch (con.typeCondition)
        {
            case 0:
                condition = "private bool" + con.conditionName;
                break;
            case 1:
                condition = "private int" + con.conditionName;
                break;
            case 2:
                condition = "private float" + con.conditionName;
                break;
            default:
                break;
        }
        return condition;
    }

    string Condition(Conditions con)
    {
        string condition = "";

        switch (con.typeCondition)
        {
            case 0:
                if (con.boolValue)
                    condition = con.conditionName + " == true";
                else
                    condition = con.conditionName + " == false";
                break;
            case 1:
                switch (con.intFloatCon)
                {
                    case 0:
                        condition = con.conditionName + " == " + con.intValue.ToString();
                        break;
                    case 1:
                        condition = con.conditionName + " > " + con.intValue.ToString();
                        break;
                    case 2:
                        condition = con.conditionName + " < " + con.intValue.ToString();
                        break;
                    case 3:
                        condition = con.conditionName + " <= " + con.intValue.ToString();
                        break;
                    case 4:
                        condition = con.conditionName + " >= " + con.intValue.ToString();
                        break;
                    case 5:
                        condition = con.conditionName + " != " + con.intValue.ToString();
                        break;
                    default:
                        break;
                }
                break;
            case 2:
                switch (con.intFloatCon)
                {
                    case 0:
                        condition = con.conditionName + " == " + con.floatValue.ToString() + "f";
                        break;
                    case 1:
                        condition = con.conditionName + " > " + con.floatValue.ToString() + "f";
                        break;
                    case 2:
                        condition = con.conditionName + " < " + con.floatValue.ToString() + "f";
                        break;
                    case 3:
                        condition = con.conditionName + " <= " + con.floatValue.ToString() + "f";
                        break;
                    case 4:
                        condition = con.conditionName + " >= " + con.floatValue.ToString() + "f";
                        break;
                    case 5:
                        condition = con.conditionName + " != " + con.floatValue.ToString() + "f";
                        break;
                    default:
                        break;
                }
                break;
            default:
                break;
        }

        return condition;
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
    public string toState = "";
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
    // 5 !=
    public int id;
}

