using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildMainBaseCommand : Command
{
    [SerializeField]
    GameObject mainBase;
    public override void Execute(SelectableObject[] active, object optionalInfo = null)
    {
        if (active[0].teamID == HumanController.GetInstance().GetTeamID())
        {
            HumanController.GetInstance().GetBuildingBuilder().SetBuildingToBuild(mainBase, (Worker)active[0]);
        }
        else
        {
            //ai
        }
    }

    public override bool IsAvailable(SelectableObject[] active)
    {
        reasonLocked = "";
        List<BuildingStats> all = Building.GetAllBuildings(active[0].teamID);
        if (Building.GetTeamMainBase(active[0].teamID) == null && mainBase.GetComponent<Building>().IsAffordable())
        {
            bool ok = true;
            foreach (Resource resource in mainBase.GetComponent<Building>().GetCosts())
            {
                if (!Player.GetPlayer(active[0].teamID).CanAffordResources(resource.type, resource.quantity))
                {
                    ok = false;
                    reasonLocked += "Need " + resource.quantity + " of " + resource.type + "\n";
                }
            }
            return ok;
        }
        else return false;
    }
}
