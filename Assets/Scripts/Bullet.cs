using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float bulletSpeed;
    private float maxDistance;
    [SerializeField] private float damage;

    private void Start()
    {
        float destroyTime = maxDistance / bulletSpeed;
        Destroy(gameObject, destroyTime);
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        transform.Translate(Vector3.forward * (bulletSpeed * Time.deltaTime));
    }

    public void SetSpeed(float speed)
    {
        bulletSpeed = speed;
    }

    public void SetMaxDistance(float distance)
    {
        maxDistance = distance;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<IDamageable>(out var damageable))
        {
            damageable.TakeDamage(damage);
            Debug.Log("Enemy damaged");
            Destroy(gameObject);
        }
    }

}