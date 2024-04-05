using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Building : SelectableObject
{
    [Header("Building")]
    [SerializeField] List<Resource> costs;
    public List<Resource> GetCosts() { return costs; }
    static Dictionary<char, List<BuildingStats>> allBuildings = new Dictionary<char, List<BuildingStats>>();
    static Dictionary<char, Building> mainBases = new Dictionary<char, Building>();
    public static List<BuildingStats> GetAllBuildings(char teamID) 
    {
        if(allBuildings.ContainsKey(teamID))
            return allBuildings[teamID];
        else return new List<BuildingStats>();
    }
    public static void AddBuilding(char teamID, Building building) 
    {
        if(!allBuildings.ContainsKey(teamID))
        {
            allBuildings.Add(teamID, new List<BuildingStats>());
        }
        allBuildings[teamID].Add(building.GetBuildingStats());
        if (building.GetBuildingStats().IsMainBase())
            mainBases.Add(teamID, building);
    }
    public static void RemoveBuilding(char teamID, Building building)
    {
        if (allBuildings.ContainsKey(teamID))
        {
            allBuildings.Remove(teamID);
        }
        if (building.GetBuildingStats().IsMainBase())
            mainBases.Remove(teamID);
    }

    public static Building GetTeamMainBase(char teamID)
    {
        if (mainBases.ContainsKey(teamID))
            return mainBases[teamID];
        else return null;
    }

    Vector3 output;
    public Vector3 GetOutput() { return output;}
    public void SetOutput(Vector3 newOutput) 
    { 
        output = newOutput;
        GetPathRender().SetPath(new List<Vector3> { transform.position + new Vector3(0,0.01f,0), newOutput });
    }

    [SerializeField]
    BuildingStats buildingStats;
    public BuildingStats GetBuildingStats() { return buildingStats;}

    [SerializeField]
    Vector2 navMeshSize;
    public Vector2 GetNavMeshSize() {  return navMeshSize; }
    public float GetNavMeshMax() { return MathF.Max(navMeshSize.x, navMeshSize.y); }

    public bool IsAffordable() 
    { 
        foreach(var cost in costs)
        {
            if (!HumanController.GetInstance().GetHumanPlayer().CanAffordResources(cost.type, cost.quantity)) return false;
        }
        return true;
    }

    public void UseResources()
    {
        foreach (var cost in costs)
        {
            HumanController.GetInstance().GetHumanPlayer().ModifyResources(cost.type, -cost.quantity);
        }
    }

    [SerializeField] UniversalQueue universalQueue;
    public UniversalQueue GetUniversalQueue() { return universalQueue;}


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, new Vector3(navMeshSize.x, 0.01f, navMeshSize.y));
    }

    float delay = 0.1f;
    float lastBuild = 0;
    private void OnTriggerStay(Collider other)
    {
        Worker w = other.GetComponent<Worker>();
        if (w != null)
        {
            lastBuild = 0.1f;
            if (delay <= 0)
            {
                ChangeCurrentHealth(w.GetBuildSpeed() * Time.deltaTime * 10f);
                delay = 0.1f;
            }
            else
            {
                delay -= Time.deltaTime;
            }
            if(GetBuildingStats().IsMainBase() || GetBuildingStats().GetResourceStorage() > 0)
            {
                if(w.IsCarrying())
                {
                    if(HumanController.GetInstance().GetHumanPlayer().ModifyResources(w.GetCarry().type, w.GetCarry().quantity));
                    {
                        w.SetCarry(null);
                        GetComponent<AudioSource>().PlayOneShot(SFX.GetInstance().dropResourceSound);
                    }
                }
            }
        }
    }
    private void Awake()
    {
        GetUniversalBar().SetMaxValue(GetCombatStats().GetMaxHealth());
        
    }

    private void Start()
    {
        selectAction += () =>
        {
            UIQueue.GetInstance().SetQueue(this);
        };
        deselectAction += () =>
        {
            UIQueue.GetInstance().ClearQueue();
        };
        output = transform.position;

        Player.GetPlayer(teamID).ModifyMaxEntities(GetBuildingStats().GetEntityAmount());
        Player.GetPlayer(teamID).ModifyMaxResources(GetBuildingStats().GetResourceStorage());
    }
    private void Update()
    {
        GetUniversalBar().SetValue(GetCurrentHealth()); 

        if(lastBuild > 0)
        {
            lastBuild -= Time.deltaTime;
            GetComponent<AudioSource>().mute = false;
        }
        else
        {
            GetComponent<AudioSource>().mute = true;
        }
    }

    private void OnDestroy()
    {

        Player.GetPlayer(teamID).ModifyMaxEntities(-GetBuildingStats().GetEntityAmount());
        Player.GetPlayer(teamID).ModifyMaxResources(-GetBuildingStats().GetResourceStorage());
    }

}
