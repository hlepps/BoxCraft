using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(Outline))]
/// SelectableObjecet-OnEnable
/// 2nd tier-Awake
/// 3rd tier-Start
public abstract class SelectableObject : MonoBehaviour
{
    [Header("SelectableObject")]
    public char teamID;

    [SerializeField] float currentHealth = 1.0f;

    public void ChangeCurrentHealth(float value)
    {
        float last = currentHealth;
        currentHealth = Mathf.Clamp(currentHealth + value, 0, combatStats.GetMaxHealth());
        if(currentHealth != last)
        {
            dmgText.DisplayDmgText(value);
            universalBar.SetValue(currentHealth);
        }

        if (currentHealth <= 0)
        {
            SFX.GetInstance().DestroySound(SFX.GetInstance().destroySelectableSound, transform.position, 1f);
            Destroy(this.gameObject);
        }
    }
    public float GetCurrentHealth() { return currentHealth; }
    public float GetCurrentHealthPercent() { return currentHealth/combatStats.GetMaxHealth(); }
    public void SetCurrentHealth(float value)
    {
        currentHealth = Mathf.Clamp(value, 0, combatStats.GetMaxHealth());
        universalBar.SetValue(currentHealth);

        if (currentHealth <= 0)
            Destroy(this.gameObject);
    }

    public void DealDamage(float attack, SelectableObject sender)
    {
        float damage = 0;
        if (attack >= combatStats.GetDefense())
        {
            damage = attack * 2 - combatStats.GetDefense();
        }
        else if (combatStats.GetDefense() != 0)
        {
            damage = attack * attack / combatStats.GetDefense();
        }
        ChangeCurrentHealth(-damage);

        if (this.GetType() == typeof(Soldier))
        {
            Soldier s = (Soldier)this;
            if (s != null)
            {
                if (s.GetAttackFocus() == null)
                {
                    s.SetAttackFocus(sender);
                    //s.CallForHeal();
                }
            }
        }
    }

    [SerializeField] CombatStats combatStats;
    public CombatStats GetCombatStats() { return combatStats; }

    [SerializeField] PathRender pathRender;
    public PathRender GetPathRender() { return pathRender; }
    public void SetPathRenderedEnabled(bool enabled)
    {
        pathRender.enabled = enabled;
    }

    [SerializeField] List<Command> commands = new List<Command>(8);
    public List<Command> GetCommands() { return commands; }

    [SerializeField] UniversalBar universalBar;
    public UniversalBar GetUniversalBar() {  return universalBar; }

    [SerializeField] DmgText dmgText;
    public DmgText GetDmgText() {  return dmgText; }

    private void OnEnable()
    {
        GetComponent<Outline>().enabled = false;
        GetComponent<Outline>().OutlineWidth = 10f;
        SetCurrentHealth(combatStats.GetMaxHealth());
    }

    protected Action selectAction, deselectAction;
    public void InvokeSelectAction() { selectAction?.Invoke(); }
    public void InvokeDeselectAction() { deselectAction?.Invoke(); }

}
