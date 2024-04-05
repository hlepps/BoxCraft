using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GatherResourceCommand : Command
{
    public override void Execute(SelectableObject[] active, object optionalInfo = null)
    {
        ResourceSource resourceSource = (ResourceSource)optionalInfo;
        ///Debug.Log(resourceSource);
        if (resourceSource == null)
            return;

        foreach (Worker w in active)
        {
            resourceSource.AddWorker(w);
            //Debug.Log(w.gameObject);

            Building whereToPlace = null; 

            List<Building> buildings = new List<Building>();
            var colliders = Physics.OverlapSphere(w.transform.position, 100);
            //Debug.Log(colliders.Length);
            foreach (var collider in colliders)
            {
                if (collider.GetComponent<Building>() != null)
                {
                    if (collider.GetComponent<Building>().GetBuildingStats().GetResourceStorage() > 0 && collider.GetComponent<SelectableObject>().teamID == '0')
                        buildings.Add(collider.GetComponent<Building>());
                }
            }
            buildings.Add(Building.GetTeamMainBase(w.teamID));

            Building closest = buildings[0];
            Debug.Log(closest.gameObject + " / " + buildings.Count);
            float closestDistance = Vector3.Distance(closest.transform.position, w.transform.position);
            foreach (Building build in buildings)
            {
                float a = Vector3.Distance(build.transform.position, w.transform.position);
                if (a < closestDistance)
                {
                    closest = build;
                    closestDistance = a;
                }
            }
            whereToPlace = closest;

            w.StartJob(
                () => 
                { 
                    if(w.IsCarrying())
                    {
                        (ResourceSource, Building) a = ((ResourceSource, Building))w.GetJobVar();
                        w.MoveWithoutStoppingJob(a.Item2);
                    }
                    else
                    {
                        if((((ResourceSource, Building))w.GetJobVar()).Item1 != null)
                        {
                            (ResourceSource, Building) a = ((ResourceSource, Building))w.GetJobVar();
                            w.MoveWithoutStoppingJob(a.Item1);
                        }
                        else
                        {
                            List<ResourceSource> srcs = new List<ResourceSource>();
                            var colliders = Physics.OverlapSphere(w.transform.position, 50);

                            ResourceSource src = null;
                            float dist = float.PositiveInfinity;

                            foreach (var collider in colliders)
                            {
                                //Debug.Log("COL " + collider.gameObject.name);
                                if (collider.GetComponent<ResourceSource>() != null)
                                {
                                    //Debug.Log("b");
                                    if (collider.GetComponent<ResourceSource>().GetResource().type == w.GetLastCarried())
                                    {
                                        //Debug.Log("c");
                                        float a = Vector3.Distance(collider.transform.position, w.transform.position);
                                        if (a < dist)
                                        {
                                            src = collider.GetComponent<ResourceSource>();
                                            dist = a;
                                            //Debug.Log("dist " + a, src.gameObject);
                                        }
                                    }
                                }
                            }
                            if (src != null)
                            {
                                src.AddWorker(w);
                                (ResourceSource, Building) x = ((ResourceSource, Building))w.GetJobVar();
                                x.Item1 = src;
                                w.SetJobVar(x);
                            }
                        }
                    }
                },
                () => { ((ResourceSource)w.GetJobVar()).RemoveWorker(w); },
                (resourceSource, whereToPlace)
            );
        }
    }


    public override bool IsAvailable(SelectableObject[] active)
    {
        if (Building.GetTeamMainBase(((Entity)active[0]).teamID))
            return true;
        else
        {

            return false;
        }
    }
}
