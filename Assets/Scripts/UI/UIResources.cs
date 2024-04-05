using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIResources : MonoBehaviour
{
    static UIResources instance;
    public static UIResources GetInstance() {  return instance; }

    [SerializeField] TextMeshProUGUI copperText; 
    [SerializeField] TextMeshProUGUI stoneText; 
    [SerializeField] TextMeshProUGUI woodText; 
    [SerializeField] TextMeshProUGUI unitsText;

    private void Awake()
    {
        if(instance != null)
            Destroy(instance.gameObject);
        instance = this;
    }

    private void UpdateCopper(int value)
    {
        copperText.text = "" + value + "/" + HumanController.GetInstance().GetHumanPlayer().GetMaxResources();
    }
    private void UpdateStone(int value)
    {
        stoneText.text = "" + value + "/" + HumanController.GetInstance().GetHumanPlayer().GetMaxResources();
    }
    private void UpdateWood(int value)
    {
        woodText.text = "" + value + "/" + HumanController.GetInstance().GetHumanPlayer().GetMaxResources();
    }
    public void UpdateType(ResourceType type, int value)
    {
        switch (type)
        {
            case ResourceType.COPPER:
                UpdateCopper(value);
                break;
            case ResourceType.STONE:
                UpdateStone(value);
                break;
            case ResourceType.WOOD:
                UpdateWood(value);
                break;
        }
    }

    public void UpdateUnits(int value, int max)
    {
        unitsText.text = value + "/" + max;
    }

    
}
