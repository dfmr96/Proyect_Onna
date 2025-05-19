using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;

public class EnemyProjectile : MonoBehaviour
{
    public float damage = 10f;
    public float lifeTime = 5f;
    protected Transform playerTransform;

    private void Start()
    {
        Destroy(gameObject, lifeTime);
        playerTransform = PlayerHelper.GetPlayer().transform;

    }



    private void OnCollisionEnter(Collision collision)
    {
   

        if ( (collision.transform == playerTransform) && (collision.gameObject.TryGetComponent<IDamageable>(out IDamageable damageable)) )
        {
            damageable.TakeDamage(damage);
            Debug.Log("Player damaged");
            Destroy(gameObject);
        }
    }
}

