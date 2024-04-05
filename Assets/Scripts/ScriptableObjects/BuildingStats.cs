using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Building stats", menuName = "ScriptableObjects/BuildingStats")]
public class BuildingStats : ScriptableObject
{
    [SerializeField] int resourceStorage;
    public int GetResourceStorage() { return resourceStorage; }

    [SerializeField] int entityAmount;
    public int GetEntityAmount() { return entityAmount; }

    [SerializeField] int howManyWorkersRequired;
    public int HowManyWorkersRequired() { return howManyWorkersRequired; }
    [SerializeField] int howManySoldiersRequired;
    public int HowManySoldiersRequired() { return howManySoldiersRequired; }
    [SerializeField] bool isMainBase;
    public bool IsMainBase() { return isMainBase; }

}
