using UnityEngine;

public abstract class Creature
{
    public string Name { get; protected set; }
    public int HP { get; protected set; }
    public int MaxHP { get; protected set; }

    public int CurrentHP => HP;
    public int Level { get; protected set; }
    public int Attack { get; protected set; }

    public Creature(string name, int hp, int attack, int level)
    {
        Name = name;
        MaxHP = hp;
        HP = hp;
        Attack = attack;
        Level = level;
    }

    public abstract void AttackTarget(Creature target);

    public virtual void TakeDamage(int damage)
    {
        HP = Mathf.Max(0, HP - damage);
    }
}