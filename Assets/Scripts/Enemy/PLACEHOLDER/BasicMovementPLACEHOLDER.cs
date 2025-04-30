using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicMovementPLACEHOLDER : MonoBehaviour, IDamageable
{
    private Renderer rend;
    private Color originalColor;

    public float speed = 5f;
    public float gravity = 9.81f;

    public float attackRange = 2f;
    public float attackDamage = 20f;
    public LayerMask enemyLayer;
    public float visionAngle = 60f;


    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;

    [field: SerializeField] public float MaxHealth { get; set; } = 100f;
    [field: SerializeField] public float CurrentHealth { get; set; }

    void Start()
    {
        controller = GetComponent<CharacterController>();
        CurrentHealth = MaxHealth;

        rend = GetComponent<Renderer>(); 
        originalColor = rend.material.color;
    }

    void Update()
    {


        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        controller.Move(move * speed * Time.deltaTime);

        velocity.y -= gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.E))
        {
            Attack();
        }
    }

    private void Attack()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, attackRange, enemyLayer);

        foreach (var hit in hits)
        {
            IDamageable enemy = hit.GetComponent<IDamageable>();
            if (enemy != null)
            {
                enemy.TakeDamage(attackDamage);
                StartCoroutine(FlashAttack());

            }
        }
    }

    public void TakeDamage(float damage)
    {
        CurrentHealth -= damage;

        //Efecto de daño
        StartCoroutine(FlashDamage());

        if (CurrentHealth <= 0)
        {
            Die();
        }

    }

    private IEnumerator FlashDamage()
    {
        rend.material.color = Color.red;
        yield return new WaitForSeconds(0.15f);
        rend.material.color = originalColor;
    }

    private IEnumerator FlashAttack()
    {
        rend.material.color = Color.blue;
        yield return new WaitForSeconds(0.15f);
        rend.material.color = originalColor;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.forward * attackRange);

        Gizmos.color = new Color(1, 0, 0, 0.2f);
        float visionAngleRad = visionAngle * Mathf.Deg2Rad;
        Vector3 leftBoundary = Quaternion.Euler(0, -visionAngle / 2f, 0) * transform.forward * attackRange;
        Vector3 rightBoundary = Quaternion.Euler(0, visionAngle / 2f, 0) * transform.forward * attackRange;
        Gizmos.DrawLine(transform.position, transform.position + leftBoundary);
        Gizmos.DrawLine(transform.position, transform.position + rightBoundary);
    }

    public void Die()
    {
        Debug.Log("Player is Dead");
        Destroy(gameObject);
    }
}
