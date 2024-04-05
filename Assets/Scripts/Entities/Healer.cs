using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healer : Entity
{
    [SerializeField] float healPower = 2;
    [SerializeField] float healRange = 4;
    [SerializeField] float healTime = 1;

    [SerializeField] TriggerOutsourcer healTrigger;

    List<Entity> nearby = new List<Entity>();
    private void Start()
    {
        Player.GetPlayer(teamID)?.AddEntityToList(this);
        healPower *= Random.Range(0.8f, 1.2f);
        healTime *= Random.Range(0.9f, 1.1f);

        healTrigger.onTriggerEnterDelegate += (Collider other) =>
        {
            Entity e = other.GetComponent<Entity>();
            if (e != null)
            {
                if (!nearby.Contains(e) && e != this && e.teamID == teamID) nearby.Add(e);
            }
        };
        healTrigger.onTriggerExitDelegate += (Collider other) =>
        {
            Entity e = other.GetComponent<Entity>();
            if (e != null)
            {
                if (nearby.Contains(e)) nearby.Remove(e);
            }
        };

        healTrigger.transform.localScale = Vector3.one * healRange*2;
    }

    float healDelay = 0;
    private void FixedUpdate()
    {
        if (nearby.Count > 0)
        {
            if (healDelay > 0)
            {
                healDelay -= Time.deltaTime;
            }
            else
            {
                healDelay = healTime;
                StartCoroutine(Heal(nearby.ToArray()));
            }
        }

        for (int i = nearby.Count - 1; i >= 0; i--)
        {
            if (nearby[i] == null)
            {
                nearby.RemoveAt(i);
            }
        }
    }

    IEnumerator Heal(Entity[] current)
    {
        Entity e = current[0];

        
        foreach (Entity ent in current)
        {
            yield return null;
            if(e.GetCurrentHealthPercent() > ent.GetCurrentHealthPercent()) e = ent;
        }
        if (e != null)
        {
            e.ChangeCurrentHealth(healPower);
            if(e.GetCurrentHealth() < e.GetCombatStats().GetMaxHealth())
                SFX.GetInstance().DestroySound(actionSound, transform.position, 2f);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1f,0.753f,0.796f,1f);
        Gizmos.DrawWireSphere(transform.position, healRange);
    }
}
