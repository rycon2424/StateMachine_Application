using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Sirenix.OdinInspector;

public class Condition : MonoBehaviour
{
    public TMP_Dropdown types;
    public TMP_Dropdown intFloatDropDown;
    public TMP_InputField valueName;
    public TMP_InputField intValue;
    public TMP_InputField floatValue;
    [Space]
    public Toggle booleanToggle;
    public Toggle isGlobal;
    [Space]
    [ReadOnly] public ConnectionBox connectionBox;
    [ReadOnly] public int id;

    private bool lockSave;

    public void RemoveSelf()
    {
        connectionBox.RemoveCondition(id);
    }

    public void SaveChange()
    {
        if (lockSave)
            return;

        connectionBox.UpdateCondition(this);
    }

    public void LoadCondition(Conditions cons)
    {
        lockSave = true;

        id = cons.id;

        valueName.text = cons.conditionName;

        intValue.text = cons.intValue;

        floatValue.text = cons.floatValue;

        booleanToggle.isOn = cons.boolValue;

        // Check if exists in global type

        types.value = cons.typeCondition;

        intFloatDropDown.value = cons.intFloatCon;

        lockSave = false;

        UpdatedType();
    }

    public void OnNameChange()
    {
        // Check if conditionName exists globally
        // if so then toggle the global bool to true
    }

    public void UpdatedType()
    {
        switch (types.value)
        {
            case 0: // Boolean
                intValue.gameObject.SetActive(false);
                floatValue.gameObject.SetActive(false);
                booleanToggle.gameObject.SetActive(true);
                intFloatDropDown.gameObject.SetActive(false);

                intValue.text = "0";
                floatValue.text = "0";

                break;
            case 1: // Integer
                intValue.gameObject.SetActive(true);
                floatValue.gameObject.SetActive(false);
                booleanToggle.gameObject.SetActive(false);
                intFloatDropDown.gameObject.SetActive(true);

                booleanToggle.isOn = true;
                floatValue.text = "0";

                break;
            case 2: // Float
                intValue.gameObject.SetActive(false);
                floatValue.gameObject.SetActive(true);
                booleanToggle.gameObject.SetActive(false);
                intFloatDropDown.gameObject.SetActive(true);

                booleanToggle.isOn = true;
                intValue.text = "0";

                break;
        }
    }
}
