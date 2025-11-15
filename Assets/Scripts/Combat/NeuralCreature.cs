using System.Collections.Generic;
using UnityEngine;

public class NeuralCreature : Creature
{
     public NeuralCreature(string name, int maxHp, int maxSp, int attack, int defense, int level, ElementType type, List<AttackData> attacks) 
         : base(name, maxHp, maxSp, attack, defense, level, type)
     {
            this.Attacks = attacks; 
     }

     public override void TakeDamage(AttackData attack, Creature attacker)
     {
         Debug.Log("Esta é a lógica de dano ESPECIAL do FireCreature/NeuralCreature!");
         
         if (attack.attackType.typeName == "Viral" && Random.Range(0, 2) == 0) 
         {
             Debug.Log("O Firewall Neural bloqueou o ataque Viral!");
         }
         else
         {
             base.TakeDamage(attack, attacker);
         }
     }
}