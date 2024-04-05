using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CheatMenu : MonoBehaviour
{
    bool cheatMenuActivated = false;
    int val = 0;
    private void Update()
    {
        if(cheatMenuActivated)
        {
            GetComponent<TextMeshProUGUI>().enabled = true;

            if(Input.GetKey(KeyCode.LeftControl))
            {
                val = 100;
            }
            else
            {
                val = 20;
            }
            if(Input.GetKeyDown(KeyCode.I))
            {
                HumanController.GetInstance().GetHumanPlayer().ModifyResources(ResourceType.COPPER, val);
            }
            if(Input.GetKeyDown(KeyCode.O))
            {
                HumanController.GetInstance().GetHumanPlayer().ModifyResources(ResourceType.STONE, val);
            }
            if(Input.GetKeyDown(KeyCode.P))
            {
                HumanController.GetInstance().GetHumanPlayer().ModifyResources(ResourceType.WOOD, val);
            }

            if(Input.GetKeyDown(KeyCode.KeypadPlus))
            {
                Time.timeScale += 1;
                Debug.Log(Time.timeScale);
            }
            if(Input.GetKeyDown(KeyCode.KeypadMinus))
            {
                if(Time.timeScale > 1)
                    Time.timeScale -= 1;
                else if(Time.timeScale > 0)
                    Time.timeScale -= 0.1f;
                if(Time.timeScale < 0)
                    Time.timeScale = 0;
                if(Time.timeScale > 0)
                    Time.timeScale = Mathf.Floor(Time.timeScale);
                Debug.Log(Time.timeScale);
            }

        }
        else
        {
            GetComponent<TextMeshProUGUI>().enabled = false;
        }


        if(Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.LeftAlt) && Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.F10)) 
        {
            cheatMenuActivated = !cheatMenuActivated;
        }
    }
}
