using UnityEngine;

[CreateAssetMenu(fileName = "EnemyStats", menuName = "Enemies/Stats")]
public class EnemyStatsSO : ScriptableObject
{
    [Header("Vitality")]
    public float MaxHealth = 100f;

    [Header("Combat")]
    public float AttackDamage = 20f;
    public bool isRangedAttack = false;
    public float AttackRange = 10f;

    [Header("Movement")]
    public float moveSpeed = 6f;
    public float rotationSpeed = 400f;
    public float RandomMovementRange = 30f;

    [Header("Rastro Orb")]
    public bool RastroOrbOnHit = true;
    public bool RastroOrbOnDeath = true;
}

