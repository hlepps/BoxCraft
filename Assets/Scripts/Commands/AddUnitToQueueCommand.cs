using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AddUnitToQueueCommand : Command
{
    [SerializeField] List<Resource> cost;
    [SerializeField] GameObject unit;
    [SerializeField] float time;
    [SerializeField] Sprite queueIcon;

    public override void Execute(SelectableObject[] active, object optionalInfo = null)
    {
        bool ok = true;
        foreach(Resource resource in cost)
        {
            if (!Player.GetPlayer(active[0].teamID).CanAffordResources(resource.type, resource.quantity))
                ok = false;
        }
        if (!Player.GetPlayer(active[0].teamID).CanHaveMoreEntities(((Building)active[0]).GetUniversalQueue().GetQueueCount())) ok = false;

        if (ok)
        {
            foreach (Resource resource in cost)
            {
                Player.GetPlayer(active[0].teamID).ModifyResources(resource.type, -resource.quantity);
            }
            ((Building)active[0]).GetUniversalQueue().AddToQueue(() =>
            {
                GameObject temp = Instantiate(unit);
                //temp.transform.position = active[0].transform.position;
                NavMeshAgent agent = temp.GetComponent<NavMeshAgent>();
                NavMeshHit hit;
                NavMesh.SamplePosition(((Building)active[0]).GetOutput(), out hit, 2f, 0);
                temp.transform.position = ((Building)active[0]).transform.position;
                agent.Warp(((Building)active[0]).transform.position);
                temp.GetComponent<Entity>().Move(((Building)active[0]).GetOutput());
                temp.GetComponent<AudioSource>().PlayOneShot(SFX.GetInstance().newUnitSound);
            },
            time, unit.name, queueIcon);
            UIQueue.GetInstance().SetQueue((Building)active[0]);
        }
    }

    public override bool IsAvailable(SelectableObject[] active)
    {
        reasonLocked = "";
        bool ok = true;
        foreach (Resource resource in cost)
        {
            if (!Player.GetPlayer(active[0].teamID).CanAffordResources(resource.type, resource.quantity))
            {
                ok = false;
                reasonLocked += "Need " + resource.quantity + " of " + resource.type + "\n";
            }
        }
        if (((Building)active[0]).GetUniversalQueue().IsQueueFull())
        {
            ok = false;
            reasonLocked += "Queue is full\n";
        }
        if (!Player.GetPlayer(active[0].teamID).CanHaveMoreEntities(((Building)active[0]).GetUniversalQueue().GetQueueCount()))
        {
            ok = false;
            reasonLocked += "You can't handle more entities!\n";
        }
        return ok;
    }
}
