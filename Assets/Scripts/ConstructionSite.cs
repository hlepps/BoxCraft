using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class ConstructionSite : SelectableObject
{
    [Header("ConstructionSite")]
    [SerializeField] GameObject toBuild;
    [SerializeField] float buildTime;
    float currentBuildTime;
    [SerializeField] float range = 0;
    char team;

    private void Awake()
    {
        GetComponent<NavMeshObstacle>().enabled = false;
        GetUniversalBar().gameObject.SetActive(false);
    }

    public void Initialize(GameObject building, float timeToBuild, char teamID)
    {
        toBuild = building;
        buildTime = timeToBuild;
        GetUniversalBar().gameObject.SetActive(true);
        GetUniversalBar().SetMaxValue(timeToBuild);
        range = building.GetComponent<Building>().GetNavMeshMax();
        team = teamID;
        currentBuildTime = 0;
        GetComponent<MeshRenderer>().material = new Material(GetComponent<MeshRenderer>().material);
        GetComponent<NavMeshObstacle>().enabled = true;

        notFinished = true;
    }

    bool notFinished = true;
    float delay = 0.1f;
    float lastBuild = 1;
    private void OnTriggerStay(Collider other)
    {
        Worker w = other.GetComponent<Worker>();
        if(w != null )
        {
            lastBuild = 0.2f;
            //Debug.Log("yes");
            if (delay <= 0)
            {
                currentBuildTime += Time.deltaTime * w.GetBuildSpeed();
                GetUniversalBar().SetValue(currentBuildTime);
                //Debug.Log(currentBuildTime + "/" + buildTime);

                if (currentBuildTime >= buildTime && notFinished)
                {
                    notFinished = false;
                    FinishBuilding();
                }
                delay = 0.1f;
            }
            else
            { delay -= Time.deltaTime; }
        }
    }
    private void FixedUpdate()
    {
        if (lastBuild > 0)
        {
            lastBuild -= Time.deltaTime;
            //GetComponent<AudioSource>().mute = false;
            Debug.Log("yes");
        }
        else
        {
            //GetComponent<AudioSource>().mute = true;
        }

    }

    List<NavMeshObstacle> obstacles = new List<NavMeshObstacle>();
    public bool IsOnObstacle() 
    {
        if (obstacles.Count <= 0)
            return false;

        for (int i = obstacles.Count - 1; i >= 0; i--)
        {
            //Debug.Log(obstacles[i].name);
            if (obstacles[i] == null)
                obstacles.RemoveAt(i);
        }
        return obstacles.Count > 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        NavMeshObstacle o = other.GetComponent<NavMeshObstacle>();
        if(o != null )
        {
            obstacles.Add(o);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        NavMeshObstacle o = other.GetComponent<NavMeshObstacle>(); 
        if (o != null && obstacles.Contains(o))
        {
            obstacles.Remove(o);
        }
    }

    public void FinishBuilding()
    {
        if (toBuild != null)
        {
            GameObject temp = GameObject.Instantiate(toBuild, transform.position, Quaternion.identity);
            temp.GetComponent<Building>().SetCurrentHealth(1);
            Building.AddBuilding('0', temp.GetComponent<Building>());
            HumanController.GetInstance().Deselect(this);
            Destroy(this.gameObject);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
