using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class PerlinNoise
{
    public static float GetNoiseValue(float x, float y, float scale, int seed)
    {
        if (scale < 0.0001f) scale = 0.0001f;
        float xCoord = seed + x / scale;
        float yCoord = seed + y / scale;
        return Mathf.PerlinNoise(xCoord, yCoord);
    }


}