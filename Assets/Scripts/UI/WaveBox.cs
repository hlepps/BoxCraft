using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WaveBox : MonoBehaviour
{
    static WaveBox instance;
    public static WaveBox GetInstance() { return instance; }
    [SerializeField] TextMeshProUGUI waveNumberText;
    [SerializeField] TextMeshProUGUI waveTimeText;

    private void Awake()
    {
        instance = this;
    }

    public void UpdateText(int waveNumber, int waveTime)
    {
        waveNumberText.text = "Wave " + waveNumber;
        waveTimeText.text = "Next wave in: " + (waveTime/60).ToString("00") + ":" + (waveTime%60).ToString("00");
    }
}
