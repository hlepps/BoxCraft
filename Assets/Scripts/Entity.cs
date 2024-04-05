using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public abstract class Entity : SelectableObject
{
    [Header("Entity")]
    NavMeshAgent agent;

    [SerializeField] float speed = 3.5f;
    [SerializeField] protected AudioClip actionSound;

    bool busy;
    Action toDo;
    Action whenfinish;
    object jobVar;
    public object GetJobVar() { return jobVar; }
    public void SetJobVar(object jv) { jobVar = jv; }

    public void FinishJob()
    {
        busy = false;
        toDo = null;
        //whenfinish?.Invoke();
    }
    public void StartJob(Action thing, Action jobFinish, object v)
    {
        FinishJob();
        toDo += thing;
        whenfinish += jobFinish;
        jobVar = v;
        busy = true;
    }
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        GetUniversalBar().SetMaxValue(GetCombatStats().GetMaxHealth());
        speed *= UnityEngine.Random.Range(0.9f, 1.1f);
        agent.speed = speed;
        agent.avoidancePriority = UnityEngine.Random.Range(40, 60);
    }
    private void OnDestroy()
    {
        Player.GetPlayer(teamID)?.RemoveEntityFromList(this);
        if(HumanController.GetInstance().GetHumanPlayer() == Player.GetPlayer(teamID))
        {
            HumanController.GetInstance().Deselect(this);
        }
    }

    public void Move(Vector3 destination)
    {
        FinishJob();
        agent.destination = destination;
    }
    public void Move(SelectableObject destination)
    {
        FinishJob(); 
        if (destination != null)
        {
            agent.destination = destination.transform.position + (transform.position - destination.transform.position).normalized / 10f;
        }
    }
    public void MoveWithoutStoppingJob(Vector3 destination)
    {
        agent.destination = destination;
    }
    public void MoveWithoutStoppingJob(SelectableObject destination)
    {
        if(destination != null)
        {
            agent.destination = destination.transform.position + (transform.position - destination.transform.position).normalized / 10f;
        }
    }
    private void Update()
    {
        toDo?.Invoke();
        GetUniversalBar().SetValue(GetCurrentHealth());

        if (agent.velocity.magnitude > 0.5f)
        {
            GetComponent<AudioSource>().mute = false;
            GetComponent<AudioSource>().pitch = Mathf.Clamp(agent.velocity.magnitude / 5f,0.9f,3f);
        }
        else
        {
            GetComponent<AudioSource>().mute = true;
        }
    }
}
