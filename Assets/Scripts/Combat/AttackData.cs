using UnityEngine;

[CreateAssetMenu(fileName = "New Attack Data", menuName = "Combat/Attack Data")]
public class AttackData : ScriptableObject
{
    public string attackName;
    public string description;

    public int damage;
    public int spCost;
    public AudioClip attackSound;

    public ElementType attackType;
}