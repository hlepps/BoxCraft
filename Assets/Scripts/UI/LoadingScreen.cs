using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LoadingScreen : MonoBehaviour
{
    public MapGen mapgen;
    public TextMeshProUGUI text;
    private void Update()
    {
        text.text = "Generating map (" + (mapgen.GetMapGenerationProgress() * 100f).ToString("00.0") + "%)";
        if(mapgen.IsMapGenerated())
        {
            Destroy(this.gameObject);
        }
    }
}
