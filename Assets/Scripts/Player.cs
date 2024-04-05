using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    static List<Player> allPlayers = new List<Player>();
    public static List<Player> GetAllPlayers() { return allPlayers; }
    public static Player GetPlayer(char teamID) { return allPlayers.Find((Player p) => { if (p.GetController().GetTeamID() == teamID) return true; else return false; }); }


    [SerializeField] Controller controller;
    public Controller GetController() { return controller; }

    Dictionary<ResourceType, int> resources = new Dictionary<ResourceType, int>();
    int maxResources = 100;
    public int GetMaxResources() { return maxResources; }
    public void ModifyMaxResources(int value)
    {
        maxResources += value;
        if (HumanController.GetInstance().GetHumanPlayer() == this)
        {
            UIResources.GetInstance().UpdateType(ResourceType.COPPER, resources[ResourceType.COPPER]);
            UIResources.GetInstance().UpdateType(ResourceType.STONE, resources[ResourceType.STONE]);
            UIResources.GetInstance().UpdateType(ResourceType.WOOD, resources[ResourceType.WOOD]);
        }
    }

    int maxEntities = 1;
    public int GetMaxEntities() { return maxEntities; }
    public void ModifyMaxEntities(int val) 
    { 
        maxEntities += val;
        UpdateUnits();
        if (HumanController.GetInstance().GetHumanPlayer() == this)
            UICommandBox.GetInstance().UpdateCommands();
    }
    public bool CanHaveMoreEntities() { return maxEntities > entities.Count; }
    public bool CanHaveMoreEntities(int additionalEntities) 
    {
        return maxEntities > entities.Count+additionalEntities; 
    }
    public int GetEntityCount() { return entities.Count; }
    public void UpdateUnits()
    {
        if (HumanController.GetInstance().GetHumanPlayer() == this)
            UIResources.GetInstance().UpdateUnits(entities.Count, maxEntities);
    }
    List<Entity> entities = new List<Entity>();
    public void AddEntityToList(Entity entity)
    {
        if(entity.teamID == controller.GetTeamID())
            entities.Add(entity);

        UpdateUnits();
        if (HumanController.GetInstance().GetHumanPlayer() == this)
            UICommandBox.GetInstance().UpdateCommands();
    }
    public List<Entity> GetAllEntities() { return entities; }
    public void RemoveEntityFromList(Entity entity) 
    { 
        if (entities.Contains(entity)) 
            entities.Remove(entity);

        UpdateUnits();
        if (HumanController.GetInstance().GetHumanPlayer() == this)
            UICommandBox.GetInstance().UpdateCommands();
    }
    private void Awake()
    {
        allPlayers.Add(this);
    }
    private void Start()
    {
        ModifyResources(ResourceType.COPPER, 100);
        ModifyResources(ResourceType.STONE, 100);
        ModifyResources(ResourceType.WOOD, 100);
    }

    public bool ModifyResources(ResourceType type, int quantity)
    {
        if(!resources.ContainsKey(type))
            resources.Add(type, 0);
        if (resources[type] + quantity > maxResources)
        {
            return false;
        }
        resources[type] += quantity;
        if (HumanController.GetInstance().GetHumanPlayer() == this)
        {
            UIResources.GetInstance().UpdateType(type, resources[type]);
            UICommandBox.GetInstance().UpdateCommands();
        }
        return true;
    }
    public void ForceModifyResources(ResourceType type, int quantity)
    {
        if (!resources.ContainsKey(type))
            resources.Add(type, 0);

        resources[type] += quantity;
        if (HumanController.GetInstance().GetHumanPlayer() == this)
        {
            UIResources.GetInstance().UpdateType(type, resources[type]);
            UICommandBox.GetInstance().UpdateCommands();
        }
    }

    public bool CanAffordResources(ResourceType type, int quantity)
    {
        return resources[type] >= quantity;
    }

    public int GetResourceQuantity(ResourceType type)
    {
        return resources[type];
    }
}
