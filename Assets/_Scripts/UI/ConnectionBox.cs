using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public class ConnectionBox : MonoBehaviour
{
    public Text transitionText;
    public Button delete;
    [Space]
    public int amountOfConditions;
    public GameObject conditionPrefab;
    public Vector3 spawnOffset;
    public float conditionOffset = 70;
    [Space]
    [ReadOnly] public Node thisNode; 
    
    public void SpawnCondition()
    {
        Condition c = Instantiate(conditionPrefab, gameObject.transform).GetComponent<Condition>();
        c.connectionBox = this;
        c.GetComponent<RectTransform>().localPosition = GetComponent<RectTransform>().localPosition - spawnOffset - new Vector3(0, 67 * thisNode.cons.Count, 0);

        Conditions cons = new Conditions();

        cons.conditionName = "Condition" + AllInfo.variableNameCounter;
        AllInfo.variableNameCounter++;
        cons.toState = thisNode.to.block.blockName;

        c.valueName.text = cons.conditionName;

        cons.id = AllInfo.instance.GetID();
        c.id = cons.id;

        thisNode.cons.Add(cons);
        Inspector.instance.ReloadConditions();
    }

    public void RemoveCondition(int id)
    {
        foreach (var condition in thisNode.cons)
        {
            if (condition.id == id)
            {
                thisNode.cons.Remove(condition);
                break;
            }
        }
        Inspector.instance.ReloadConditions();
    }

    public void LoadCondition(Conditions con, int order)
    {
        Condition c = Instantiate(conditionPrefab, gameObject.transform).GetComponent<Condition>();
        c.connectionBox = this;
        c.GetComponent<RectTransform>().localPosition -= new Vector3(0, 67 * order, 0);
        c.LoadCondition(con);
    }

    public void UpdateCondition(Condition condition)
    {
        foreach (var con in thisNode.cons)
        {
            //Debug.Log($"{con.id} / {condition.id}");
            if (con.id == condition.id)
            {
                SaveCondition(condition, con);
                break;
            }
        }
    }

    void SaveCondition(Condition from, Conditions to)
    {
        to.conditionName = from.valueName.text;
        to.typeCondition = from.types.value;
        to.boolValue = from.booleanToggle.isOn;
        to.intValue = from.intValue.text;
        to.floatValue = from.floatValue.text;
        to.intFloatCon = from.intFloatDropDown.value;
        to.isGlobal = from.isGlobal.isOn;
    }

}
