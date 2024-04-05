using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIQueueEntry : MonoBehaviour
{
    [SerializeField] Image icon;
    [SerializeField] TextMeshProUGUI name;
    [SerializeField] Slider slider;

    public void Init(Sprite iconSprite, string nameText, float makeTime)
    {
        icon.sprite = iconSprite;
        name.text = nameText;
        slider.maxValue = makeTime;
    }

    public bool SetValue(float value)
    {
        slider.value = value;
        if(slider.value >= slider.maxValue) 
        {
            return true;
        }
        return false;
    }
}
