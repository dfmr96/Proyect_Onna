using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RastroOrb : MonoBehaviour
{
    public static event Action<float> OnOrbCollected;
    private Action _onCollected;

    public float floatSpeed = 0.5f;
    public float floatAmplitude = 0.5f;
    public float healingAmount = 10f;
    public float lifetime = 5f;

    private Transform attractionTarget;
    public LayerMask attractionLayer; 
    public float attractionRadius = 5f;
    public float attractionSpeed = 5f;

    private float timer;
    private Vector3 startPos;

    private void OnEnable()
    {
        timer = lifetime;
        attractionTarget = null;
    }

    void Start() { startPos = transform.position; }

    void Update()
    {
        //Flotado en Idle
        float newY = startPos.y + Mathf.Sin(Time.time * floatSpeed) * floatAmplitude;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);

        timer -= Time.deltaTime;

         if (timer <= 0f)
         {
            //Si en el tiempo no lo toma el player se desactiva del Pool
            DeactivateOrb();
         }

        if (attractionTarget == null) CheckForAttraction();
        else
        {
            float distance = Vector3.Distance(transform.position, attractionTarget.position);

            //Si el jugador se alejo deja de seguirlo
            if (distance > attractionRadius)
            {
                attractionTarget = null;
                return;
            }

            //Sigue al jugador
            Vector3 dir = (attractionTarget.position - transform.position).normalized;
            transform.position += dir * attractionSpeed * Time.deltaTime;
        }
    }

    private void CheckForAttraction()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, attractionRadius, attractionLayer);

        foreach (Collider hit in hits)
        {
            if (hit.CompareTag("Player"))
            {
                attractionTarget = hit.transform;
                break;
            }
        }
    }

    public void Init(System.Action onCollected) { _onCollected = onCollected; }

    private void DeactivateOrb()
    {
        attractionTarget = null;
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    { 
        //IDamageable damageable = other.GetComponent<IDamageable>();
        IHealable healable = other.GetComponent<IHealable>();

        if (healable != null)
        {
            //damageable.CurrentHealth += healingAmount;
            //if (damageable.CurrentHealth > damageable.MaxHealth)
            //    damageable.CurrentHealth = damageable.MaxHealth;
            healable.RecoverTime(healingAmount);

            OnOrbCollected?.Invoke(healingAmount);
            _onCollected?.Invoke();
        }

    }

}
