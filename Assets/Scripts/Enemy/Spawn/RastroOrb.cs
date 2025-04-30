using System;
using System.Collections.Generic;
using Player;
using UnityEngine;

namespace Enemy.Spawn
{
    public class RastroOrb : MonoBehaviour
    {
        public static event Action<float> OnOrbCollected;

        [Header("Floating Settings")]
        [SerializeField] private float floatSpeed = 0.5f;
        [SerializeField] private float floatAmplitude = 0.5f;
    
        [Header("Healing Settings")]
        [SerializeField] private float healingAmount = 10f;
    
        [Header("Lifetime Settings")]
        [SerializeField] private float lifetime = 5f;

        [Header("Attraction Settings")]
        [SerializeField] private float attractionRadius = 5f;
        [SerializeField] private float attractionSpeed = 5f;
        
        private Transform attractionTarget;
        private Action _onCollected;
        private float timer;
        private Vector3 startPos;
        private GameObject playerGO;
    
        //----------------------------------------------------------------------
        // Unity Methods
        //----------------------------------------------------------------------
        private void OnEnable()
        {
            timer = lifetime;
            attractionTarget = null;
        }

        void Start()
        {
            startPos = transform.position;
            GetPlayer();
        }
        
        void Update()
        {
            timer -= Time.deltaTime;
            HandleFloating();
            HandleLifetime();
            HandleAttraction();
        }
    
        private void OnTriggerEnter(Collider other)
        { 
            if (!IsPlayer(other)) return;
            HealPlayer();
            OnOrbCollected?.Invoke(healingAmount);
            _onCollected?.Invoke();
            DeactivateOrb();
        }
    
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, attractionRadius);
        }
    
        //----------------------------------------------------------------------
        // Public methods
        //----------------------------------------------------------------------
        public void Init(Action onCollected)
        {
            _onCollected = onCollected;
        }
    
        //----------------------------------------------------------------------
        // Private methods
        //----------------------------------------------------------------------
    
        private void GetPlayer()
        {
            playerGO = PlayerHelper.GetPlayer();

            if (playerGO == null)
            {
                Debug.LogWarning("[ORB] No player found. Attraction will not work.");
            }
        }
        
        private void HandleFloating()
        {
            Vector3 position = transform.position;
            position.y = startPos.y + Mathf.Sin(Time.time * floatSpeed) * floatAmplitude;
            transform.position = position;
        }
        private void HandleLifetime()
        {
            if (timer <= 0f)
            {
                //Si en el tiempo no lo toma el player se desactiva
                DeactivateOrb();
            }
        }
    
        private void HandleAttraction()
        {
            if (attractionTarget == null)
            {
                TryStartAttraction();
            }
            else
            {
                MoveTowardsAttractionTarget();
            }
        }
    
        private void TryStartAttraction()
        {
            if (playerGO == null)
                return;

            float distance = Vector3.Distance(transform.position, playerGO.transform.position);
            if (distance <= attractionRadius)
            {
                attractionTarget = playerGO.transform;
            }
        }
    
        private void MoveTowardsAttractionTarget()
        {
            float distance = Vector3.Distance(transform.position, attractionTarget.position);
            if (distance > attractionRadius)
            {
                attractionTarget = null;
                return;
            }

            Vector3 direction = (attractionTarget.position - transform.position).normalized;
            transform.Translate(direction * (attractionSpeed * Time.deltaTime), Space.Self);
        }
        private bool IsPlayer(Collider other)
        {
            return other.gameObject == playerGO;
        }
        private void HealPlayer()
        {
            if (playerGO.TryGetComponent(out IHealable healable))
            {
                healable.RecoverTime(healingAmount);
            }
        }

        private void DeactivateOrb()
        {
            attractionTarget = null;
            gameObject.SetActive(false);
        }
    }
}
