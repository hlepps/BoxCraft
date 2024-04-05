using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectionsBox : MonoBehaviour
{
    static SelectionsBox instance;
    public static SelectionsBox GetInstance() {  return instance; }

    [SerializeField] TextMeshProUGUI selectionsText;

    private void Awake()
    {
        instance = this;
    }

    public void UpdateText(List<SelectableObject>[] selections)
    {
        string text = "";
        for(int i = 0; i < selections.Length; i++)
        {
            text += "F" + i + "  (" + selections[i].Count + ")" + "\n";
        }
        selectionsText.text = text;
    }
}
