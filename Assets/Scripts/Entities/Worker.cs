using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Worker : Entity
{
    [Header("Worker")]
    [SerializeField] float buildSpeed = 10;
    public float GetBuildSpeed() { return buildSpeed; }


    Resource carry;
    ResourceType lastCarried;
    public ResourceType GetLastCarried() { return lastCarried; }

    public void SetCarry(Resource res)
    {
        carry = res;
        if(res != null)
            lastCarried = res.type;
    }

    public bool IsCarrying() { return carry != null && carry?.quantity > 0; }

    public Resource GetCarry()
    {
        return carry;
    }

    private void Start()
    {
        Player.GetPlayer(teamID)?.AddEntityToList(this);
        buildSpeed *= Random.Range(0.8f, 1.2f);
    }
}
