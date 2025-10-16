using UnityEngine;

public class FireCreature : Creature
{
     public FireCreature(string name, int hp, int attack, int level) : base(name, hp, attack, level) {}

    public override void AttackTarget(Creature target)
    {
        int damage = Attack;
        target.TakeDamage(damage);
        Debug.Log($"{Name} atacou {target.Name} causando {damage} de dano!");
    }
}

