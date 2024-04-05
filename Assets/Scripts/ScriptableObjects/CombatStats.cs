using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Combat stats", menuName = "ScriptableObjects/CombatStats")]
public class CombatStats : ScriptableObject
{
    [SerializeField] float maxHealth;
    public float GetMaxHealth() { return maxHealth; }

    [SerializeField] float attack;
    public float GetAttack() { return attack; }
    [SerializeField][Range(0,1)] float critChance = 0.05f;
    public float GetCritChance() { return critChance; }
    [SerializeField] float critPower = 2;
    public float GetCritPower() { return critPower; }

    [SerializeField][Min(1)] float defense;
    public float GetDefense() { return defense;}


}
