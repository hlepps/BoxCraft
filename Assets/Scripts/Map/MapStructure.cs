using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MapStructure : MonoBehaviour
{
    [SerializeField] protected GameObject toPlant;

    public abstract void PlantOnMap(MapGen map, Vector2 size, int seed);
    
}
