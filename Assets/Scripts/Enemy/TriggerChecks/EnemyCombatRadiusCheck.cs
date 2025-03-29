using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCombatRadiusCheck : MonoBehaviour
{
    public GameObject PlayerTarget { get; set; }
    private EnemyController _enemyController;

    private void Awake()
    {
        PlayerTarget = GameObject.FindGameObjectWithTag("Player");

        _enemyController = GetComponentInParent<EnemyController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == PlayerTarget)
        {
            _enemyController.SetCombatRadiusBool(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == PlayerTarget)
        {
            _enemyController.SetCombatRadiusBool(false);

        }
    }
}
