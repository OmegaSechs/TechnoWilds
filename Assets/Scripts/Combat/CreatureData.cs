using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NovoMonstro", menuName = "Monster Core/Monstro")]
public class CreatureData : ScriptableObject
{
    [Header("Info Básica")]
    public string creatureName;
    public string creatureInfo;
    public ElementType creatureType; 
    public GameObject battlePrefab; 

    [Header("Stats Base (Nível 1)")]
    public int maxHealth;
    public int maxSP;
    public int baseAttack;
    public int baseDefense;
    
    public int baseLevel; 

    [Header("Ataques")]
    public List<AttackData> attacks; 
}