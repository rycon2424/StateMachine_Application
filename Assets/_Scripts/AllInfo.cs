using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Linq;

public class AllInfo : MonoBehaviour
{
    [ReadOnly] public string projectName;
    [ReadOnly] public string stateMachineName;
    [ReadOnly] public string mainClass;
    [ReadOnly] public string onEnterFunctionName;
    [ReadOnly] public string onExitFunctionName;
    [ReadOnly] public string onUpdateFunctionName;

    public List<Block> blocks = new List<Block>();
    [SerializeField] List<int> takenIDs = new List<int>();

    public static AllInfo instance;
    public static int variableNameCounter;

    private void Awake()
    {
        if (instance != null)
            Destroy(instance);
        instance = this;
    }

    public bool IsOnlyBlock() // If only block it will automaticly be the enterState
    {
        if (blocks.Count < 1)
        {
            return true;
        }
        return false;
    }

    public void RemoveBlock(Block blockToRemove)
    {
        blocks.Remove(blockToRemove);
        if (blocks.Count > 0)
        {
            if (blockToRemove.enterState == true)
            {
                int newEnterBlock = Random.Range(0, blocks.Count);
                blocks[newEnterBlock].UpdateEnterState(true);
            }
        }
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
        ApplicationConsole.console.UpdateConsole("Exported to " + pathForExport);
        Debug.Log("Exported to " + pathForExport);
    }

    void CreateMainClass(string path)
    {
        string mainClassScript = path + $"/{mainClass}.cs";
        if (!File.Exists(mainClassScript))
        {
            string scriptContent = "";

            scriptContent += "using System.Collections;@using System.Collections.Generic;@using UnityEngine;";
            scriptContent += "@";
            scriptContent += "@public class " + mainClass + " : MonoBehaviour";
            scriptContent += "@{@";
            scriptContent += "#public " + stateMachineName + " stateMachine;";
            scriptContent += "@#public State currentState;";

            List<string> globalVariables = new List<string>();
            foreach (Block state in blocks)
            {
                foreach (Node node in state.connections)
                {
                    foreach (Conditions con in node.cons)
                    {
                        if (con.isGlobal)
                        {
                            if (!globalVariables.Contains(con.conditionName))
                            {
                                scriptContent += "@#" + Variable(con, "public") + ";";
                                globalVariables.Add(con.conditionName);
                            }
                        }
                    }
                }
            }

            scriptContent += "@@#void Start() @#{@## stateMachine = new " + stateMachineName + "();@##SetupStateMachine(); @#}";

            scriptContent += "@@#void SetupStateMachine() @#{";
            int s = 0;
            foreach (Block state in blocks)
            {
                s++;
                scriptContent += "@##" + state.blockName + " state" + s.ToString() + " = new " + state.blockName + "();";
                scriptContent += "@##stateMachine.AddState(state" + s.ToString() + ");";
            }
            scriptContent += "@##stateMachine.GoToState(this, state" + s.ToString() + ");";
            scriptContent += "@#}@";

            scriptContent += "@#void Update()@#{@##currentState." + onUpdateFunctionName + "(this);@#}@";

            scriptContent += "@#public void ChangeState(State newState)@#{@## currentState = newState;@#}@}";

            File.WriteAllText(mainClassScript, PlaceTabsAndNewlines(scriptContent));
        }
    }

    void CreateDefaultState(string path)
    {
        string mainClassScript = path + $"/State.cs";
        if (!File.Exists(mainClassScript))
        {
            string scriptContent = "";

            scriptContent += "using System.Collections;@using System.Collections.Generic;@using UnityEngine;";
            scriptContent += "@";
            scriptContent += "@public class State@{";
            scriptContent += "@#public virtual void " + onEnterFunctionName + "(" + mainClass + " actor) @#{@#}@";
            scriptContent += "@#public virtual void " + onExitFunctionName + "(" + mainClass + " actor) @#{@#}@";
            scriptContent += "@#public virtual void " + onUpdateFunctionName + "(" + mainClass + " actor) @#{@#}@";
            scriptContent += "@}";

            File.WriteAllText(mainClassScript, PlaceTabsAndNewlines(scriptContent));
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
            scriptContent += "@#private State currentState;";
            scriptContent += "@#private List<State> allStates = new List<State>();";
            scriptContent += "@#bool stateExists;";

            //GoToState Function
            scriptContent += "@@#public void GoToState(" + mainClass + " actor, string newstate)@#{";
            scriptContent += "@##if (LockStateMachine)@##{@###return;@## }";
            scriptContent += "@##stateExists = false;";
            scriptContent += "@##foreach (var s in allStates)@##{@###if (s.GetType().ToString() == newstate)@###{@####stateExists = true;@####break;@###}@##}";
            scriptContent += "@##if (stateExists == false)@##{@###return;@##}";
            scriptContent += "@##if (currentState != null)@##{##@### currentState." + onExitFunctionName + "(actor); @##}";
            scriptContent += "@##foreach (var s in allStates)@##{@###if (s.GetType().ToString() == newstate)@###{";
            scriptContent += "@####currentState = s;@@####actor.ChangeState(currentState);@@####s." + onEnterFunctionName + "(actor);@####return;@###}@##}@#}";

            // GetSet Lock Machine
            scriptContent += "@@#public bool LockStateMachine";
            scriptContent += "@#{";
            scriptContent += "@##get;";
            scriptContent += "@##set;";
            scriptContent += "@#}";

            // AddState
            scriptContent += "@@#public void AddState(State newState)";
            scriptContent += "@#{";
            scriptContent += "@##allStates.Add(newState);";
            scriptContent += "@#}";

            // CurrentState
            scriptContent += "@@#public State CurrentState()";
            scriptContent += "@#{";
            scriptContent += "@##return currentState;";
            scriptContent += "@#}";

            // IsInState
            scriptContent += "@@#public bool IsInState(string state)";
            scriptContent += "@#{";
            scriptContent += "@##if (state == currentState.GetType().ToString())";
            scriptContent += "@##{";
            scriptContent += "@###return true;";
            scriptContent += "@##}";
            scriptContent += "@##return false;";
            scriptContent += "@#}";
            scriptContent += "@}";

            File.WriteAllText(mainClassScript, PlaceTabsAndNewlines(scriptContent));
        }
    }

    void CreateState(string path, Block className)
    {
        string mainClassScript = path + $"/{className.blockName}.cs";
        if (!File.Exists(mainClassScript))
        {
            string scriptContent = "";
            scriptContent += "using System.Collections;@using System.Collections.Generic;@using UnityEngine;";
            scriptContent += $"@@ public class {className.blockName} : State";
            scriptContent += "@{@";

            // Variables
            foreach (Node node in className.connections)
            {
                foreach (Conditions condition in node.cons)
                {
                    if (condition.isGlobal == false)
                    {
                        scriptContent += Variable(condition, "#private");
                        scriptContent += ";";
                        scriptContent += "@";
                    }
                }
            }

            scriptContent += "@#public override void " + onEnterFunctionName + "(" + mainClass + " actor)@#{@@#}@";
            scriptContent += "@#public override void " + onExitFunctionName + "(" + mainClass + " actor)@#{@@#}@";
            scriptContent += "@#public override void " + onUpdateFunctionName + "(" + mainClass + " actor)@#{@";


            // Conditions
            foreach (Node node in className.connections)
            {
                if (node.cons.Count > 0)
                {
                    scriptContent += "@##if (";
                    for (int i = 0; i < node.cons.Count; i++)
                    {
                        if (node.cons[i].isGlobal == true)
                        {
                            scriptContent += Condition(node.cons[i], "actor.");
                        }
                        else
                        {
                            scriptContent += Condition(node.cons[i]);
                        }

                        if (i + 1 < node.cons.Count)
                        {
                            scriptContent += " && ";
                        }
                    }
                    scriptContent += ")@";
                    scriptContent += "##{";

                    const string quote = "\"";

                    scriptContent += "@###actor.stateMachine.GoToState(actor, " + quote + node.to.block.blockName + quote + ");";

                    scriptContent += "@##}@";
                }
            }

            scriptContent += "@#}@";
            scriptContent += "@}@";

            File.WriteAllText(mainClassScript, PlaceTabsAndNewlines(scriptContent));
        }
    }

    string Variable(Conditions con, string access)
    {
        string condition = "";

        switch (con.typeCondition)
        {
            case 0:
                condition = access + " bool " + con.conditionName;
                break;
            case 1:
                condition = access + " int " + con.conditionName;
                break;
            case 2:
                condition = access + " float " + con.conditionName;
                break;
            default:
                break;
        }
        return condition;
    }

    string Condition(Conditions con, string inheritedClass = "")
    {
        string condition = "";

        switch (con.typeCondition)
        {
            case 0:
                if (con.boolValue)
                    condition = inheritedClass + con.conditionName + " == true";
                else
                    condition = inheritedClass + con.conditionName + " == false";
                break;
            case 1:
                switch (con.intFloatCon)
                {
                    case 0:
                        condition = inheritedClass + con.conditionName + " == " + con.intValue.ToString();
                        break;
                    case 1:
                        condition = inheritedClass + con.conditionName + " > " + con.intValue.ToString();
                        break;
                    case 2:
                        condition = inheritedClass + con.conditionName + " < " + con.intValue.ToString();
                        break;
                    case 3:
                        condition = inheritedClass + con.conditionName + " <= " + con.intValue.ToString();
                        break;
                    case 4:
                        condition = inheritedClass + con.conditionName + " >= " + con.intValue.ToString();
                        break;
                    case 5:
                        condition = inheritedClass + con.conditionName + " != " + con.intValue.ToString();
                        break;
                    default:
                        break;
                }
                break;
            case 2:
                switch (con.intFloatCon)
                {
                    case 0:
                        condition = inheritedClass + con.conditionName + " == " + con.floatValue.ToString() + "f";
                        break;
                    case 1:
                        condition = inheritedClass + con.conditionName + " > " + con.floatValue.ToString() + "f";
                        break;
                    case 2:
                        condition = inheritedClass + con.conditionName + " < " + con.floatValue.ToString() + "f";
                        break;
                    case 3:
                        condition = inheritedClass + con.conditionName + " <= " + con.floatValue.ToString() + "f";
                        break;
                    case 4:
                        condition = inheritedClass + con.conditionName + " >= " + con.floatValue.ToString() + "f";
                        break;
                    case 5:
                        condition = inheritedClass + con.conditionName + " != " + con.floatValue.ToString() + "f";
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

    string PlaceTabsAndNewlines(string scriptContent)
    {
        scriptContent = scriptContent.Replace("@", System.Environment.NewLine);
        scriptContent = scriptContent.Replace("#", "\t");

        return scriptContent;
    }
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
    public bool isGlobal = false;
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

