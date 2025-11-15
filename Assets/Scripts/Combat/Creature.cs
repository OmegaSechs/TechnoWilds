using System.Collections.Generic;
using UnityEngine;

public abstract class Creature
{
    public string Name { get; protected set; }
    public int MaxHP { get; protected set; }
    public int MaxSP { get; protected set; }
    public int Level { get; protected set; }
    public int Attack { get; protected set; } 
    public int Defense { get; protected set; }
    
    public ElementType CreatureType { get; protected set; } 

    private int currentHP;
    private int currentSP;
    public int CurrentHP => currentHP;
    public int CurrentSP => currentSP;
    public List<AttackData> Attacks { get; protected set; }

    public Creature(string name, int hp, int sp, int attack, int defense, int level, ElementType type)
    {
        Name = name;
        MaxHP = hp;
        MaxSP = sp;
        Attack = attack;
        Defense = defense;
        Level = level;
        currentHP = hp; 
        currentSP = sp;
        CreatureType = type; 
    }

    public virtual void AttackTarget(Creature target, AttackData attack)
    {        
        Debug.Log($"{this.Name} usa {attack.attackName} em {target.Name}!");

        target.TakeDamage(attack, this);

        this.currentSP -= attack.spCost;
        this.currentSP = Mathf.Max(0, this.currentSP);
    }

    public virtual void TakeDamage(AttackData attack, Creature attacker)
    {
        int baseDamage = (attack.damage + attacker.Attack) - this.Defense;

        float typeMultiplier = 1.0f; 

        if (this.CreatureType != null && attack.attackType != null)
        {
            if (this.CreatureType.weaknesses.Contains(attack.attackType))
            {
                typeMultiplier = 2.0f;
                Debug.Log("Foi super efetivo!");
            }
            if (this.CreatureType.resistances.Contains(attack.attackType))
            {
                typeMultiplier = 0.5f; 
                Debug.Log("Não foi muito efetivo...");
            }
        }

        int finalDamage = Mathf.RoundToInt(baseDamage * typeMultiplier);
        finalDamage = Mathf.Max(1, finalDamage); 

        currentHP = Mathf.Max(0, currentHP - finalDamage);
    }
}