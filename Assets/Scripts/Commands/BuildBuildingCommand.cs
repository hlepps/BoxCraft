using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildBuildingCommand : Command
{
    [SerializeField]
    GameObject building;
    public override void Execute(SelectableObject[] active, object optionalInfo = null)
    {
        if (active[0].teamID == HumanController.GetInstance().GetTeamID())
        {
            HumanController.GetInstance().GetBuildingBuilder().SetBuildingToBuild(building, (Worker)active[0]);
        }
        else
        {
            //ai
        }
    }

    public override bool IsAvailable(SelectableObject[] active)
    {
        bool ok = true;
        reasonLocked = "";
        if (Building.GetTeamMainBase(active[0].teamID) == null) return false;

        foreach (Resource resource in building.GetComponent<Building>().GetCosts())
        {
            if (!Player.GetPlayer(active[0].teamID).CanAffordResources(resource.type, resource.quantity))
            {
                ok = false;
                reasonLocked += "Need " + resource.quantity + " of " + resource.type + "\n";
            }
        }
        return ok;
    }
}
