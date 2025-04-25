using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationAttack : MonoBehaviour
{

    private Transform _playerTransform;
    private EnemyController _enemyController;


    private void Start()
    {
        _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        _enemyController = GetComponent<EnemyController>();


    }
    public void AnimationAttackFunc()
    {
        IDamageable damageablePlayer = _playerTransform.GetComponent<IDamageable>();
        _enemyController.ExecuteAttack(damageablePlayer);
    }


}
