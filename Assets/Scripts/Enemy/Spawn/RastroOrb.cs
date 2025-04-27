using System;
using System.Collections.Generic;
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
        [SerializeField] private LayerMask attractionLayer; 
        [SerializeField] private float attractionRadius = 5f;
        [SerializeField] private float attractionSpeed = 5f;

    
        [Header("Debug")]
        [SerializeField] private List<Collider> colliders = new List<Collider>();

        private Transform attractionTarget;
        private Action _onCollected;
        private float timer;
        private Vector3 startPos;
    
        //----------------------------------------------------------------------
        // Unity Methods
        //----------------------------------------------------------------------
        private void OnEnable()
        {
            timer = lifetime;
            attractionTarget = null;
        }

        void Start() { startPos = transform.position; }

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
            HealPlayer(other);
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
        public void Init(Action onCollected) { _onCollected = onCollected; }
    
        //----------------------------------------------------------------------
        // Private methods
        //----------------------------------------------------------------------
    
        private void HandleFloating()
        {
            float newY = startPos.y + Mathf.Sin(Time.time * floatSpeed) * floatAmplitude;
            transform.position = new Vector3(transform.position.x, newY, transform.position.z);
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
                SearchForAttractionTarget();
            }
            else
            {
                MoveTowardsAttractionTarget();
            }
        }
    
        private void SearchForAttractionTarget()
        {
            Collider[] hits = Physics.OverlapSphere(transform.position, attractionRadius, attractionLayer);
            colliders = new List<Collider>(hits);

            foreach (Collider hit in hits)
            {
                attractionTarget = hit.transform; // No es necesario CheckTag porque ya usamos LayerMask
                break;
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
            return other.CompareTag("Player") && other.GetComponent<IHealable>() != null;
        }
        private void HealPlayer(Collider playerCollider)
        {
            IHealable healable = playerCollider.GetComponent<IHealable>();
            healable?.RecoverTime(healingAmount);
        }

        private void DeactivateOrb()
        {
            attractionTarget = null;
            gameObject.SetActive(false);
        }
    }
}
