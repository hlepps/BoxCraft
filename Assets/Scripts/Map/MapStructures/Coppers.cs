using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.Rendering;

public class Coppers : MapStructure
{
    Texture2D text;
    private int[,] whereToPlant;
    private int[,] actualPlaces;
    private Color[] pix;
    public override void PlantOnMap(MapGen map, Vector2 size, int seed)
    {
        seed -= seed * (int)Mathf.Cos(seed);
        text = new Texture2D((int)size.x, (int)size.y);
        pix = new Color[(int)size.x * (int)size.y];
        whereToPlant = new int[(int)size.x,(int)size.y];
        actualPlaces = new int[(int)size.x,(int)size.y];
        for (int y = 0; y < size.y; y++)
        {
            for (int x = 0; x < size.x; x++)
            {
                float sample =
                    PerlinNoise.GetNoiseValue(x, y, 40, seed) * 0.6f +
                    PerlinNoise.GetNoiseValue(x, y, 80, seed * 2) * 0.3f +
                    PerlinNoise.GetNoiseValue(x, y, 65, seed * 4) * 0.1f;

                if(sample < 0.40)
                {
                    whereToPlant[x, y] = 1;
                    float t = Random.Range(0f, 1f);
                    if (t < 0.06f)
                        actualPlaces[x, y] = 1;
                    else
                        actualPlaces[x, y] = 0;
                }
                else
                {
                    whereToPlant[x, y] = 0;
                    actualPlaces[x, y] = 0;
                }

                //sample /= step;
                //sample = Mathf.Floor(sample);
                //sample *= step;

                //before[(int)y, (int)x] = sample;
            }
        }

        StartCoroutine(PlantTrees(size, map));
        //PlantTrees(size,map);
        
    }
    IEnumerator PlantTrees(Vector2 size, MapGen map)
    {
        //yield return null;
        for (int y = 0; y < size.y; y++)
        {
            yield return null;
            for (int x = 0; x < size.x; x++)
            {
                if (whereToPlant[x, y] == 1)
                {
                    if (actualPlaces[x, y] == 1)
                    {
                        float height = map.GetHeightNotScaled(x, y);
                        if (height > 0.55f)
                        {
                            var temp = Instantiate(toPlant, map.transform);
                            temp.transform.localPosition = new Vector3(x, map.GetHeight(x,y), y);
                        }
                        pix[y * (int)size.x + x] = new Color(0, 1, 0);
                    }
                }
                else
                    pix[y * (int)size.x + x] = new Color(0, 0, 0);

            }
        }
        text.SetPixels(pix);
        //File.WriteAllBytes("koperki.png", text.EncodeToPNG());
        text.Apply();
        //Debug.Log("Saved stone");
    }
}
