using System.Collections.Generic;
using UnityEngine;

public abstract class Creature
{
public string Name { get; protected set; }
    public int MaxHP { get; protected set; }
    public int MaxSP { get; protected set; }
    public int Level { get; protected set; }
    public int Attack { get; protected set; } 
    private int currentHP;
    private int currentSP;

    public int CurrentHP => currentHP;
    public int CurrentSP => currentSP;


    public List<AttackData> Attacks { get; protected set; }

    public Creature(string name, int hp, int sp, int attack, int level)
    {
        Name = name;
        MaxHP = hp;
        MaxSP = sp;
        Attack = attack;
        Level = level;
        currentHP = hp; 
        currentSP = sp;
        
        Attacks = new List<AttackData>();
    }

public virtual void AttackTarget(Creature target, AttackData attack)
    {        
        Debug.Log($"{this.Name} usa {attack.attackName} em {target.Name}!");

        int damage = attack.damage + this.Attack;
        target.TakeDamage(damage);


        this.currentSP -= attack.spCost;
        this.currentSP = Mathf.Max(0, this.currentSP);
    }
    public virtual void TakeDamage(int damage)
    {
        currentHP = Mathf.Max(0, currentHP - damage);
    }
}
