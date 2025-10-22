using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NovoMonstro", menuName = "Biomon/Monstro")]
public class CreatureData : ScriptableObject
{
    [Header("Info Básica")]
    public string creatureName;
    public string creatureInfo;
    public ElementType creatureType;
    public GameObject battlePrefab; // O prefab com o sprite e animações

    [Header("Stats Base (Nível 1)")]
    public int maxHealth;
    public int maxSP;
    public int baseAttack;
    public int baseDefense;

    [Header("Ataques")]
    public List<AttackData> attacks; 
}