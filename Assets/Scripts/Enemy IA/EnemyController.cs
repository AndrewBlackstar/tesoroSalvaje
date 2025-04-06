using UnityEngine;

[RequireComponent(typeof(Animator))]
public class NPCPatrolChaseCombat : MonoBehaviour
{
    public float patrolSpeed = 2f;
    public float chaseSpeed = 4f;
    public float patrolDistance = 5f;
    public float chaseRange = 10f;
    public float attackRange = 1.5f;
    public float attackCooldown = 2f;
    public int damage = 10;
    public Transform player;

    private Animator animator;
    private Vector3 startPosition;
    private float lastAttackTime;

    void Start()
    {
        animator = GetComponent<Animator>();
        startPosition = transform.position;
        animator.SetBool("isWalking", true);
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= attackRange)
        {
            EngageCombat();
        }
        else if (distanceToPlayer <= chaseRange)
        {
            ChasePlayer();
        }
        else
        {
            Patrol();
        }
    }

    void Patrol()
    {
        animator.SetBool("isWalking", true);
        animator.SetBool("isInCombat", false);

        Vector3 direction = transform.forward;
        float step = patrolSpeed * Time.deltaTime;
        transform.Translate(direction * step, Space.World);

        float distanceFromStart = Vector3.Distance(startPosition, transform.position);
        if (distanceFromStart >= patrolDistance)
        {
            transform.Rotate(0f, 180f, 0f);
            startPosition = transform.position;
        }
    }

    void ChasePlayer()
    {
        animator.SetBool("isWalking", true);
        animator.SetBool("isInCombat", false);

        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0f;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * 5f);
        transform.position += direction * chaseSpeed * Time.deltaTime;
    }

    void EngageCombat()
    {
        animator.SetBool("isWalking", false);
        animator.SetBool("isInCombat", true);

        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0f;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * 5f);

        if (Time.time >= lastAttackTime + attackCooldown)
        {
            lastAttackTime = Time.time;
            animator.SetTrigger("Attack");

            PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }
        }
    }
}
