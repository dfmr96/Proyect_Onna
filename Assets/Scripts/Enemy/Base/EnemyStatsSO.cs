using UnityEngine;

[CreateAssetMenu(fileName = "Enemy Logic", menuName = "Enemy Stats/Enemy Stats Base")]
public class EnemyStatsSO : ScriptableObject
{
    [Header("Vitality")]
    public float MaxHealth = 100f;

    [Header("Combat")]
    public float AttackDamage = 20f;
    public float AttackRange = 10f;

    [Header("Ranged Combat")]
    public float ShootForce = 15f;

    [Header("Movement")]
    public float moveSpeed = 6f;
    public float rotationSpeed = 400f;
    public float RandomMovementRange = 30f;

    [Header("Rastro Orb")]
    public bool RastroOrbOnHit = true;
    public bool RastroOrbOnDeath = true;
}

