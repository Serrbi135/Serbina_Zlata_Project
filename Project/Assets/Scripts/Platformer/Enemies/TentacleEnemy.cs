using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;


public class FlyingEnemy : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float chaseDistance = 10f;
    [SerializeField] private float attackDistance = 5f;
    [SerializeField] private float stoppingDistance = 2f;

    [Header("Attack Settings")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float attackCooldown = 2f;
    [SerializeField] private float projectileSpeed = 7f;

    [Header("Tail Settings")]
    //[SerializeField] private int tailLength = 10;
    [SerializeField] private float tailSmoothness = 0.5f;
    [SerializeField] private float tailWidth = 0.2f;
    [SerializeField] private Gradient tailColor;
    [SerializeField] private float tailUpdateRate = 0.05f;

    private Transform player;
    private float lastAttackTime;
    private Rigidbody2D rb;
    private Animator animator;
    private bool isFacingRight = true;

    // Tail variables
    private LineRenderer tailRenderer;
    private Vector3[] tailPositions;
    private Vector3[] tailVelocity;
    private float tailUpdateTimer;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        lastAttackTime = -attackCooldown;

        //InitializeTail();
    }

   /* private void InitializeTail()
    {
        // Create and configure LineRenderer
        tailRenderer = gameObject.AddComponent<LineRenderer>();
        tailRenderer.material = new Material(Shader.Find("Sprites/Default"));
        tailRenderer.widthCurve = AnimationCurve.Linear(0, tailWidth, 1, 0);
        tailRenderer.colorGradient = tailColor;
        tailRenderer.positionCount = tailLength;

        // Initialize position arrays
        tailPositions = new Vector3[tailLength];
        tailVelocity = new Vector3[tailLength];

        // Fill with current position
        for (int i = 0; i < tailLength; i++)
        {
            tailPositions[i] = transform.position;
            tailVelocity[i] = Vector3.zero;
        }
    */

    private void Update()
    {
        if (player == null) return;

        HandleMovement();
        HandleAttack();
        //UpdateTail();
    }

    private void HandleMovement()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= chaseDistance && distanceToPlayer > stoppingDistance)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            rb.velocity = direction * moveSpeed;

            if (direction.x > 0 && !isFacingRight)
            {
                Flip();
            }
            else if (direction.x < 0 && isFacingRight)
            {
                Flip();
            }

            
        }
        else
        {
            rb.velocity = Vector2.zero;

            
        }
    }

    private void HandleAttack()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= attackDistance && Time.time >= lastAttackTime + attackCooldown)
        {
            Attack();
            lastAttackTime = Time.time;
        }
    }

    /*private void UpdateTail()
    {
        tailUpdateTimer += Time.deltaTime;

        if (tailUpdateTimer >= tailUpdateRate)
        {
            tailUpdateTimer = 0f;

            // Shift positions in the array
            for (int i = tailPositions.Length - 1; i > 0; i--)
            {
                tailPositions[i] = Vector3.SmoothDamp(tailPositions[i], tailPositions[i - 1], ref tailVelocity[i], tailSmoothness);
            }

            // Set first position to enemy position
            tailPositions[0] = transform.position;

            // Apply to LineRenderer
            tailRenderer.SetPositions(tailPositions);
        }
    }*/

    private void Attack()
    {
        if (projectilePrefab == null || firePoint == null) return;

        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D projectileRb = projectile.GetComponent<Rigidbody2D>();

        Vector2 direction = (player.position - firePoint.position).normalized;
        projectileRb.velocity = direction * projectileSpeed;

        
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, chaseDistance);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackDistance);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, stoppingDistance);
    }
}
