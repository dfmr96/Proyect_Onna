using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationAttack : MonoBehaviour
{

    private Transform _playerTransform;
    private EnemyController _enemyController;
    private float _distanceToCountExit = 3f;



    private void Start()
    {
        //_playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        _playerTransform = PlayerHelper.GetPlayer().transform;

        _enemyController = GetComponent<EnemyController>();


    }
    public void AnimationAttackFunc()
    {

        if (_playerTransform != null)
        {
            float distanceToPlayer = Vector3.Distance(_playerTransform.position, transform.position);

            //doble comprobacion por si se aleja
            if (distanceToPlayer <= _distanceToCountExit)
            {
                IDamageable damageablePlayer = _playerTransform.GetComponent<IDamageable>();
                _enemyController.ExecuteAttack(damageablePlayer);
            }
        }

        
    }


}
