using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAggroCheck : MonoBehaviour
{
    [Header("Vision Cone Stats")]
    [SerializeField] private float detectionRange = 10f; 
    [SerializeField] private float visionAngle = 45f; 
    [SerializeField] private LayerMask obstacleLayers; 

    private EnemyController _enemyController;
    private Transform _playerTransform;

    private void Awake()
    {
        _enemyController = GetComponentInParent<EnemyController>();
        _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        CheckForPlayer();
    }

    private void CheckForPlayer()
    {
        if (_playerTransform == null) return;

        Vector3 directionToPlayer = (_playerTransform.position - transform.position).normalized;
        float distanceToPlayer = Vector3.Distance(transform.position, _playerTransform.position);

        if (distanceToPlayer > detectionRange)
        {
            _enemyController.SetAggroStatus(false);
            return;
        }

        float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);
        if (angleToPlayer > visionAngle / 2)
        {
            _enemyController.SetAggroStatus(false);
            return;
        }

        if (!Physics.Raycast(transform.position, directionToPlayer, distanceToPlayer, obstacleLayers))
        {
            _enemyController.SetAggroStatus(true);
        }
        else
        {
            _enemyController.SetAggroStatus(false);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (_playerTransform == null) return;

        Gizmos.color = Color.red;
        Vector3 rightBoundary = Quaternion.Euler(0, visionAngle / 2, 0) * transform.forward;
        Vector3 leftBoundary = Quaternion.Euler(0, -visionAngle / 2, 0) * transform.forward;

        Gizmos.DrawLine(transform.position, transform.position + rightBoundary * detectionRange);
        Gizmos.DrawLine(transform.position, transform.position + leftBoundary * detectionRange);
    }
}
