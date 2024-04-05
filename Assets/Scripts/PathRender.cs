using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(LineRenderer))]
public class PathRender : MonoBehaviour
{
    public bool renderNavMeshPath;
    [SerializeField]
    Material pathMaterial;
    
    LineRenderer lineRenderer;
    NavMeshAgent agent;


    List<Vector3> path;
    public void SetPath(List<Vector3> newPath)
    {
        path = newPath;
    }
    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        pathMaterial = Instantiate(pathMaterial);
        if(renderNavMeshPath)
        {
            agent = transform.parent.GetComponent<NavMeshAgent>();
        }
        this.enabled = false;
    }

    private void Update()
    {
        lineRenderer.materials = new Material[1];
        lineRenderer.material = pathMaterial;
        if (renderNavMeshPath)
        {
            lineRenderer.positionCount = agent.path.corners.Length;
            lineRenderer.SetPositions(agent.path.corners);
            //lineRenderer.material.SetFloat("_LineWidth", Useful.GetPathLength(agent.path.corners));
        }
        else if (path != null)
        {
            lineRenderer.positionCount = path.Count;
            lineRenderer.SetPositions(path.ToArray());
        }
    }

    private void OnEnable()
    {
        lineRenderer.enabled = true;
    }

    private void OnDisable()
    {
        lineRenderer.enabled = false;
    }
}
