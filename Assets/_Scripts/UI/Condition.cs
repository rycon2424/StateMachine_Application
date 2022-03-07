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
    public TMP_InputField intFloatValue;
    [Space]
    public Toggle booleanToggle;
    public Toggle isGlobal;
    [Space]
    [ReadOnly] public ConnectionBox connectionBox;

    public void SetupCondition(int type, string conName)
    {

    }

    public void UpdatedType()
    {
        intFloatValue.text = "";
        booleanToggle.isOn = true;
        switch (types.value)
        {
            case 0: // Boolean
                intFloatValue.gameObject.SetActive(false);
                booleanToggle.gameObject.SetActive(true);
                intFloatDropDown.gameObject.SetActive(false);
                break;
            case 1: // Integer
                intFloatValue.gameObject.SetActive(true);
                booleanToggle.gameObject.SetActive(false);
                intFloatDropDown.gameObject.SetActive(true);

                intFloatValue.contentType = TMP_InputField.ContentType.IntegerNumber;
                break;
            case 2: // Float
                intFloatValue.gameObject.SetActive(true);
                booleanToggle.gameObject.SetActive(false);
                intFloatDropDown.gameObject.SetActive(true);

                intFloatValue.contentType = TMP_InputField.ContentType.DecimalNumber;
                break;
        }
    }
}
