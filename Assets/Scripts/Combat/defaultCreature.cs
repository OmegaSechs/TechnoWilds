using System.Collections.Generic;
using UnityEngine;

public class defaultCreature : Creature
{
     public defaultCreature(string name, int maxHp, int maxSp, int attack, int defense, int level, ElementType type, List<AttackData> attacks) 
         : base(name, maxHp, maxSp, attack, defense, level, type)
     {
            this.Attacks = attacks; 
     }
}