using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldier : Entity
{
    protected float critChanceModifier = 1;

    [SerializeField] protected bool attackMode = true;
    [SerializeField] protected float attackModeRange = 3;
    [SerializeField] protected float attackTime = 0.5f;

    [SerializeField] protected TriggerOutsourcer attackTrigger;
    protected List<SelectableObject> nearby = new List<SelectableObject>();
    protected List<Healer> healers = new List<Healer>();

    protected SelectableObject attackFocus = null;
    public void SetAttackFocus(SelectableObject newFocus)
    {
        attackFocus = newFocus;
    }
    public void CallForHeal()
    {

    }
    public SelectableObject GetAttackFocus() { return attackFocus; }

    public void ToggleAttackMode(bool atk)
    {
        attackMode = atk;
    }
    public void ToggleAttackMode() { attackMode = !attackMode; }
    public bool GetAttackMode() { return attackMode; }

    private void Start()
    {
        Player.GetPlayer(teamID)?.AddEntityToList(this);
        critChanceModifier = Random.Range(0.8f, 1.2f);

        attackTrigger.onTriggerEnterDelegate += (Collider other) =>
        {
            SelectableObject e = other.GetComponent<SelectableObject>();
            if (e != null)
            {
                if (e.teamID != this.teamID && e.teamID != 'n')
                {
                    if (!nearby.Contains(e) && e != this) nearby.Add(e);
                }
                if(e.teamID == this.teamID && e.GetType() == typeof(Healer))
                {
                    //if (!healers.Contains((Healer)e)) healers.Add((Healer)e);
                }
            }
        };
        attackTrigger.onTriggerExitDelegate += (Collider other) =>
        {
            SelectableObject e = other.GetComponent<SelectableObject>();
            if (e != null)
            {
                if (nearby.Contains(e)) nearby.Remove(e);
                //if(healers.Contains((Healer)e)) healers.Remove((Healer)e);
            }
        };

        attackTrigger.transform.localScale = Vector3.one * attackModeRange * 2;
    }

    protected float attackDelay = 0;
    private void FixedUpdate()
    {
        if (attackMode)
        {
            if(attackFocus == null)
            {
                if (nearby.Count > 0)
                {
                    if (attackDelay <= 0)
                    {
                        attackDelay = attackTime * Random.Range(0.9f,1.1f);
                        StartCoroutine(Attack(nearby.ToArray()));
                    }
                    else
                    {
                        attackDelay -= Time.deltaTime;
                    }
                }
            }
            else
            {
                if (attackDelay <= 0)
                {
                    attackDelay = attackTime * Random.Range(0.9f, 1.1f);
                    StartCoroutine(Attack(new SelectableObject[]{attackFocus}));
                }
                else
                {
                    attackDelay -= Time.deltaTime;
                }
            }
        }

        for(int i = nearby.Count-1; i >= 0; i--)
        {
            if (nearby[i] == null)
            {
                nearby.RemoveAt(i);
            }
        }

    }
    protected virtual IEnumerator Attack(SelectableObject[] current)
    {
        SelectableObject s = current[0];
        float dist = 0;
        if (s != null)
            dist = Vector3.Distance(transform.position, s.transform.position);
        foreach (SelectableObject sel in current)
        {
            yield return null;
            if (sel != null)
            {
                if (Vector3.Distance(transform.position, sel.transform.position) < dist) s = sel;
            }
        }
        if (s != null)
        {
            Move(s);
            while (!nearby.Contains(s))
                yield return null;
            /*while(Vector3.Distance(transform.position, s.transform.position) > attackModeRange)
            {
                yield return null;
            }*/
            bool isCrit = Random.Range(0f, 1f) < GetCombatStats().GetCritChance() * critChanceModifier;
            if (isCrit)
            {
                if (s != null)
                    s.DealDamage(GetCombatStats().GetAttack() * GetCombatStats().GetCritPower(), this);
            }
            else
            {
                if (s != null)
                    s.DealDamage(GetCombatStats().GetAttack(), this);
            }
            SFX.GetInstance().DestroySound(actionSound, transform.position, 2f);
        }
    }

    private void OnDrawGizmos()
    {
        if (attackMode)
        {
            Gizmos.color = Color.yellow;
        }
        else
        {
            Gizmos.color = Color.red;
        }

        Gizmos.DrawWireSphere(transform.position, attackModeRange);
    }
}
