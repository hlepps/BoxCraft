using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class Chunk : MonoBehaviour
{
    Mesh mesh;
    Vector3[] newVertices;
    Vector2[] newUV;
    int[] newTriangles;

    [SerializeField] int width;
    [SerializeField] int height;
    [SerializeField] float xOffset;
    [SerializeField] float yOffset;
    public void SetOffset(float x, float y)
    {
        xOffset = x;
        yOffset = y;
    }

    int seed = 0;
    public void SetSeed(int seed)
    { this.seed = seed; }

    [SerializeField] float scale = 100f;
    [SerializeField] float step = 0.2f;
    [SerializeField] float stepMeshMultiplier = 10.0f;

    private Texture2D noiseTex;
    private float[,] before;
    private float[,] after;
    private Color[] pix;
    private Renderer rend;

    private NavMeshSurface navMesh;
    private MeshCollider collider;

    [SerializeField] List<Color> colors;

    [SerializeField] bool liveUpdateMesh = false;

    bool isGenerated = false;
    public bool IsGenerated() { return isGenerated; }

    public float GetHeight(int x, int y)
    {
        return after[x, y] * stepMeshMultiplier;
    }
    public float GetHeightNotScaled(int x, int y)
    {
        return after[x, y];
    }

    int generatedVertices = 0;
    public float GetGeneratedChunkProgress()
    {
        return (float)generatedVertices / ((float)height * (float)width);
    }

    public void Generate()
    {
        //tekstura
        rend = GetComponent<Renderer>();
        noiseTex = new Texture2D(width, height);
        pix = new Color[noiseTex.width * noiseTex.height];
        before = new float[(noiseTex.width*3),(noiseTex.height*3)];
        after = new float[width,height];
        CalcNoise();


        navMesh = GetComponent<NavMeshSurface>();
        collider = GetComponent<MeshCollider>();
        //mesh
        StartCoroutine(CalcMesh());


        

    }
    IEnumerator CalcMesh()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        mesh.Clear();

        newVertices = new Vector3[(width + 1) * (height + 1)];
        newUV = new Vector2[newVertices.Length]; 
        Vector4[] tangents = new Vector4[newVertices.Length];
        Vector4 tangent = new Vector4(1f, 0f, 0f, -1f);
        for (int i = 0, z = 0; z <= height; z++)
        {
            for (int x = 0; x <= width; x++, i++)
            {
                int tx = x;
                if (x == width)
                    tx = width - 1;
                int tz = z;
                if (z == height)
                    tz = height - 1;
                newVertices[i] = new Vector3(x, after[tx,tz]*stepMeshMultiplier, z);
                newUV[i] = new Vector2((float)x / width, (float)z / height); 
                tangents[i] = tangent;
                
            }
        }
        mesh.vertices = newVertices;
        mesh.uv = newUV; 
        mesh.tangents = tangents;

        newTriangles = new int[width * height * 6];
        for (int ti = 0, vi = 0, y = 0; y < height; y++, vi++)
        {
            yield return null;
            for (int x = 0; x < width; x++, ti += 6, vi++)
            {
                newTriangles[ti] = vi;
                newTriangles[ti + 3] = newTriangles[ti + 2] = vi + 1;
                newTriangles[ti + 4] = newTriangles[ti + 1] = vi + width + 1;
                newTriangles[ti + 5] = vi + width + 2;
                generatedVertices++; 

                if (liveUpdateMesh && x % 10 == 0)
                {
                    mesh.triangles = newTriangles;
                    yield return null;
                }
            }
        }
        mesh.triangles = newTriangles;

        mesh.RecalculateNormals();

        //collider
        collider.sharedMesh = null;
        collider.sharedMesh = mesh;

        //navmesh
        navMesh.BuildNavMesh();
        isGenerated = true;
    }
    void CalcNoise()
    {
        // For each pixel in the texture...
        float y = 0.0F;

        while (y < height*3)
        {
            float x = 0.0F;
            while (x < width*3)
            {
                float sample =
                    PerlinNoise.GetNoiseValue(x + xOffset / 3f, y + yOffset / 3f, 100, seed) * 0.6f +
                    PerlinNoise.GetNoiseValue(x + xOffset / 3f, y + yOffset / 3f, 50, seed * 2) * 0.3f +
                    PerlinNoise.GetNoiseValue(x + xOffset / 3f, y + yOffset / 3f, 25, seed * 4) * 0.1f;


                //stepowanie
                sample /= step;
                sample = Mathf.Floor(sample);
                sample *= step;

                before[(int)y, (int)x] = sample;
                x++;
            }
            y++;
        }

        for(int y2 = 0; y2 < before.GetLength(0); y2++)
        {
            for(int x2 = 0; x2 < before.GetLength(1); x2++)
            {
                int nx = x2 / 3;
                int ny = y2 / 3;
                after[nx, ny] += before[x2, y2];
            }
        }
        for (int y2 = 0; y2 < after.GetLength(0); y2++)
        {
            for (int x2 = 0; x2 < after.GetLength(1); x2++)
            {
                after[x2, y2] = after[x2,y2] / 9f;
                float val = after[x2, y2];
                
                if(val >= 0.8f)
                    pix[y2 * width + x2] = colors[4];
                else if (val >= 0.5f)
                    pix[y2 * width + x2] = colors[3];
                else if (val >= 0.4f)
                    pix[y2 * width + x2] = colors[2];
                else if(val >= 0.2f)
                    pix[y2 * width + x2] = colors[1];
                else
                    pix[y2 * width + x2] = colors[0];

            }
        }

        noiseTex.SetPixels(pix);
        noiseTex.Apply();
        rend.material.mainTexture = noiseTex;
        
    }

}
