using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ranged : Soldier
{
    [SerializeField] GameObject projectile;
    [SerializeField] float projectileSpeed = 8f;
    protected override IEnumerator Attack(SelectableObject[] current)
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
            GameObject temp = Instantiate(projectile);
            temp.transform.position = transform.position;
            SFX.GetInstance().DestroySound(actionSound, transform.position, 2f);
            StartCoroutine(FireProjectile(temp, s));
            
        }
    }

    IEnumerator FireProjectile(GameObject projectile, SelectableObject target)
    {
        Vector3 lastKnown = target.transform.position;
        projectile.GetComponent<Projectile>().Init();
        while (Vector3.Distance(projectile.transform.position, lastKnown) > 0.1f)
        {
            yield return null;
            projectile.GetComponent<Projectile>().Sustain();
            if (target != null)
            { 
                lastKnown = target.transform.position;
            }

            projectile.transform.position += (lastKnown - projectile.transform.position).normalized * projectileSpeed * Time.deltaTime;
        }
        projectile.GetComponent<Projectile>().Finish();

        bool isCrit = Random.Range(0f, 1f) < GetCombatStats().GetCritChance() * critChanceModifier;
        if (isCrit)
        {
            if (target != null)
                target.DealDamage(GetCombatStats().GetAttack() * GetCombatStats().GetCritPower(), this);
        }
        else
        {
            if (target != null)
                target.DealDamage(GetCombatStats().GetAttack(), this);
        }
    }
}
