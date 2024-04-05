using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.AI;

public class BuildingBuilder : MonoBehaviour
{
    [SerializeField]
    LayerMask hitLayerMask;
    [SerializeField]
    GameObject constructionSite;
    [SerializeField]
    Command fixCommand;

    [SerializeField] AudioClip startBuildSound;

    GameObject toBuild = null;
    Worker currentWorker = null;
    public bool IsBuilding()
    {
        return toBuild != null;
    }
    public void SetBuildingToBuild(GameObject building, Worker worker)
    {
        currentWorker = worker;
        toBuild = building;
        constructionSite.transform.localScale = new Vector3(toBuild.GetComponent<Building>().GetNavMeshSize().x, 1, toBuild.GetComponent<Building>().GetNavMeshSize().y);
    }

    Vector3[] positions = new Vector3[4];
    void Update()
    {
        if(toBuild != null)
        {
            Ray ray = CameraManager.GetInstance().GetCameraComponent().ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 100, hitLayerMask))
            {
                constructionSite.SetActive(true);
                constructionSite.transform.position = hit.point + Vector3.up*constructionSite.transform.localScale.y/2f;
                bool isOk = true;
                for(int i = 0; i < 4; i++)
                {
                    positions[i] = hit.point;
                    positions[i].y += 0.1f;
                    if (i == 0)
                    {
                        positions[i].x -= toBuild.GetComponent<Building>().GetNavMeshSize().x / 2;
                        positions[i].z -= toBuild.GetComponent<Building>().GetNavMeshSize().y / 2;
                    }
                    if (i == 1)
                    {
                        positions[i].x += toBuild.GetComponent<Building>().GetNavMeshSize().x / 2;
                        positions[i].z -= toBuild.GetComponent<Building>().GetNavMeshSize().y / 2;
                    }
                    if (i == 2)
                    {
                        positions[i].x -= toBuild.GetComponent<Building>().GetNavMeshSize().x / 2;
                        positions[i].z += toBuild.GetComponent<Building>().GetNavMeshSize().y / 2;
                    }
                    if (i == 3)
                    {
                        positions[i].x += toBuild.GetComponent<Building>().GetNavMeshSize().x / 2;
                        positions[i].z += toBuild.GetComponent<Building>().GetNavMeshSize().y / 2;
                    }

                    if (NavMesh.SamplePosition(positions[i], out NavMeshHit hit2, 0.25f, NavMesh.AllAreas) && !constructionSite.GetComponent<ConstructionSite>().IsOnObstacle())
                    {
                    }
                    else
                    {
                        //Debug.Log("Is in obstacle: " + constructionSite.GetComponent<ConstructionSite>().IsOnObstacle());
                        //Debug.Log("positions["+i+"]: " + positions[i] + " " + hit2.position);
                        isOk = false;
                    }
                }

                if(isOk)
                {
                    constructionSite.GetComponent<MeshRenderer>().material.color = Color.green;
                    if(Input.GetMouseButtonDown(0))
                    {
                        StartCoroutine(SendToBuild(toBuild, hit.point, currentWorker));
                        toBuild = null;
                    }

                }
                else
                {
                    constructionSite.GetComponent<MeshRenderer>().material.color = Color.red;
                }
                
            }
        }
        else
        {
            constructionSite.SetActive(false);
        }

        if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Escape))
        {
            toBuild = null;
        }
        

    }

    IEnumerator SendToBuild(GameObject building, Vector3 position, Worker worker)
    {
        worker.Move(position);
        float val = building.GetComponent<Building>().GetNavMeshMax();
        while (Vector3.Distance(worker.transform.position, position) > val)
        {
            yield return null;
        }
        if (building.GetComponent<Building>().IsAffordable())
        {
            GetComponent<AudioSource>().PlayOneShot(startBuildSound);
            building.GetComponent<Building>().UseResources();
            GameObject placeholder = GameObject.Instantiate(constructionSite, position, Quaternion.identity);
            placeholder.SetActive(true);
            ConstructionSite site = placeholder.GetComponent<ConstructionSite>();
            worker.Move(site);
            site.Initialize(building, building.GetComponent<Building>().GetCombatStats().GetMaxHealth() / 100f, '0');
            //site.enabled = true;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        foreach(Vector3 vec in positions)
        {
            Gizmos.DrawSphere(vec, 1);
        }
    }
}
