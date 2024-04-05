using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixBuildingCommand : Command
{
    [SerializeField]
    GameObject mainBase;
    public override void Execute(SelectableObject[] active, object optionalInfo = null)
    {
        //HumanController.GetInstance().GetBuildingBuilder().SetBuildingToBuild(mainBase, (Worker)active[0]);

        //non existent
    }

    public override bool IsAvailable(SelectableObject[] active)
    {
        List<BuildingStats> all = Building.GetAllBuildings(active[0].teamID);
        foreach (BuildingStats b in all)
        {
            if (b.IsMainBase())
                return false;
        }
        return true;
    }
}
