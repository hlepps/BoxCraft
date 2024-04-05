using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGen : MonoBehaviour
{
    [SerializeField] GameObject chunkPrefab;
    [SerializeField] List<GameObject> chunks = new List<GameObject>();
    [SerializeField] Vector2 size = new Vector2(3, 3);

    [SerializeField] List<MapStructure> mapStructures;

    [SerializeField] int seed;

    public float GetHeight(int x, int y)
    {
        return chunks[0].GetComponent<Chunk>().GetHeight(x, y);
    }
    public float GetHeightNotScaled(int x, int y)
    {
        return chunks[0].GetComponent<Chunk>().GetHeightNotScaled(x, y);
    }
    void Start()
    {
        seed = Random.Range(0, 99999);

        // jeden czank zeby prosciej bylo
        for (int x = 0; x < size.x; x++ )
        {
            for(int y = 0; y < size.y; y++ )
            {
                GameObject temp = Instantiate( chunkPrefab );
                temp.transform.SetParent(this.transform);
                temp.transform.localPosition = new Vector3(100 * x, 0, 100 * y);
                temp.GetComponent<Chunk>().SetOffset(100 * x, 100 * y);
                temp.GetComponent<Chunk>().SetSeed(seed);
                temp.GetComponent<Chunk>().Generate();
                chunks.Add(temp);
            }
        }
        StartCoroutine(GenerateResources());
    }

    public bool IsMapGenerated()
    {
        foreach (var chunk in chunks)
        {
            if (!chunk.GetComponent<Chunk>().IsGenerated()) return false;
        }
        return true;
    }

    public float GetMapGenerationProgress()
    {
        float progress = 0;
        foreach(var chunk in chunks)
        {
            progress += chunk.GetComponent<Chunk>().GetGeneratedChunkProgress();
        }
        progress /= chunks.Count;
        return progress;
    }

    bool resourcesGenerated = false;
    public bool AreResourcesGenerated() { return resourcesGenerated; }
    IEnumerator GenerateResources()
    {
        foreach(MapStructure mapStructure in mapStructures)
        {
            yield return null;
            mapStructure.PlantOnMap(this, new Vector2(200, 200), seed);
        }
        resourcesGenerated = true;
    }
}
