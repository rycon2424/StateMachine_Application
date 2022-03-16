using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ApplicationConsole : MonoBehaviour
{
    [SerializeField] TMP_Text text;
    public static ApplicationConsole console;

    private void Awake()
    {
        if (console != null)
            Destroy(console);
        console = this;
    }

    public void UpdateConsole(string newText)
    {
        text.text = newText;
    }
}
