using System.Collections.Generic;
using UnityEngine;

public class FireCreature : Creature
{
     public FireCreature(string name, int maxHp, int maxSp, int attack, int level, List<AttackData> attacks) : base(name, maxHp, maxSp, attack, level)
        {
            this.Attacks = attacks;
    }
}

