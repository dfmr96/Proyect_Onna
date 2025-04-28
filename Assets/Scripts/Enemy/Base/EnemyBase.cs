using Enemy.Spawn;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{

    #region Vitality
    [field: SerializeField] public float MaxHealth { get; set; } = 100f;
    public float CurrentHealth { get; set; }
    #endregion

    #region Combat Stats
    public float AttackDamage = 5f;
    //Redundante resolver tomarlo del inspector o al reves
    public bool isRangedAttack;
    public float AttackRange = 10f;
    #endregion

    #region Movement
    public float moveSpeed = 5f;
    public float rotationSpeed = 400f;
    public float RandomMovementRange = 30f;
    #endregion

    #region RastroOrb
    public bool RastroOrbOnHit = true;
    public bool RastroOrbOnDeath = true;
    #endregion

}

