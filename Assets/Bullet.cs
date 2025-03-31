using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class Bullet : MonoBehaviour
{
    private float bulletSpeed; 
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
}