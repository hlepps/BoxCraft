using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionManager : MonoBehaviour
{
    [SerializeField] List<SelectableObject>[] selections = new List<SelectableObject>[8];

    private void Start()
    {
        for(int i = 0; i < selections.Length; i++)
        {
            selections[i] = new List<SelectableObject>();
        }
    }
    List<KeyCode> codes = new List<KeyCode>(){ KeyCode.F1, KeyCode.F2, KeyCode.F3, KeyCode.F4, KeyCode.F5, KeyCode.F6, KeyCode.F7, KeyCode.F8 };
    void Update()
    {
        if(Input.GetKey(KeyCode.LeftControl)) 
        {
            foreach (KeyCode code in codes)
            {
                if (Input.GetKeyDown(code))
                {
                    selections[codes.IndexOf(code)] = new List<SelectableObject>();
                    var temp = HumanController.GetInstance().GetSelectionObjects();
                    foreach(SelectableObject s in temp)
                    {
                        if(s != null)
                        {
                            selections[codes.IndexOf(code)].Add(s);
                        }
                    }
                }
            }
        }
        else
        {
            foreach (KeyCode code in codes)
            {
                if (Input.GetKeyDown(code))
                {
                    HumanController.GetInstance().SetSelectionObjects(selections[codes.IndexOf(code)]);
                }
            }
        }
        SelectionsBox.GetInstance().UpdateText(selections);
    }
}
