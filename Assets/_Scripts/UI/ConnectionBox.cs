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

        cons.conditionName = "Condition" + "(" + thisNode.cons.Count +")";

        c.valueName.text = cons.conditionName;

        thisNode.cons.Add(cons);
        Inspector.instance.ReloadConditions();
    }

    public void LoadCondition(Conditions con, int order)
    {
        Condition c = Instantiate(conditionPrefab, gameObject.transform).GetComponent<Condition>();
        c.connectionBox = this;
        c.GetComponent<RectTransform>().localPosition = GetComponent<RectTransform>().localPosition - spawnOffset - new Vector3(0, conditionOffset * order, 0);

    }

    public void UpdateCondition(Condition condition)
    {
        foreach (var con in thisNode.cons)
        {
            if (con.conditionName == condition.valueName.text)
            {
                CopyValues(condition, con);
                break;
            }
        }
    }

    void CopyValues(Condition from, Conditions to)
    {
        to.conditionName = from.valueName.text;
        to.typeCondition = from.types.value;
        to.boolValue = from.booleanToggle.isOn;
        to.intValue =   int.Parse(from.intFloatValue.text);
        to.floatValue = float.Parse(from.intFloatValue.text);
        to.intFloatCon = from.intFloatDropDown.value;
    }

}
