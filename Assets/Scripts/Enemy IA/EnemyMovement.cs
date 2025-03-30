using UnityEngine;
using System.Collections;

public class EnemyMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 3f;
    public float minWaitTime = 1f;
    public float maxWaitTime = 3f;
    public float patrolRadius = 5f;
    public float stoppingDistance = 0.5f;
    public float obstacleDetectionDistance = 2f; // Distancia para detectar obstáculos

    private Vector3 initialPosition;
    private Vector3 targetPosition;
    private Rigidbody rb;
    private bool isWaiting = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        initialPosition = transform.position;
        SetNewRandomTarget();
    }

    void FixedUpdate()
    {
        if (isWaiting) return;

        if (HasObstacleInPath()) // Si hay obstáculo, cambia de dirección
        {
            SetNewRandomTarget();
            return;
        }

        MoveToTarget();
        CheckIfReachedTarget();
    }

    bool HasObstacleInPath()
    {
        Vector3 directionToTarget = (targetPosition - transform.position).normalized;
        
        // Raycast hacia el objetivo
        if (Physics.Raycast(transform.position, directionToTarget, out RaycastHit hit, obstacleDetectionDistance))
        {
            if (!hit.collider.CompareTag("Player")) // Ignora al jugador (si lo tienes)
            {
                Debug.DrawRay(transform.position, directionToTarget * hit.distance, Color.red, 1f); // Debug visual
                return true;
            }
        }
        return false;
    }

    void MoveToTarget()
    {
        Vector3 direction = (targetPosition - transform.position).normalized;
        direction.y = 0f;
        rb.linearVelocity = direction * moveSpeed;
    }

    void CheckIfReachedTarget()
    {
        float distanceToTarget = Vector3.Distance(
            new Vector3(transform.position.x, 0f, transform.position.z),
            new Vector3(targetPosition.x, 0f, targetPosition.z)
        );

        if (distanceToTarget <= stoppingDistance)
        {
            StartCoroutine(WaitAndSetNewTarget());
        }
    }

    IEnumerator WaitAndSetNewTarget()
    {
        isWaiting = true;
        rb.linearVelocity = Vector3.zero;

        yield return new WaitForSeconds(Random.Range(minWaitTime, maxWaitTime));

        SetNewRandomTarget();
        isWaiting = false;
    }

    void SetNewRandomTarget()
    {
        int attempts = 0;
        Vector3 newTarget;
        bool targetIsValid = false;

        // Intenta encontrar una dirección sin obstáculos (máx. 5 intentos)
        while (attempts < 5 && !targetIsValid)
        {
            Vector2 randomPoint = Random.insideUnitCircle * patrolRadius;
            newTarget = initialPosition + new Vector3(randomPoint.x, 0f, randomPoint.y);
            
            Vector3 directionToNewTarget = (newTarget - transform.position).normalized;
            
            // Verifica si la nueva dirección está libre
            if (!Physics.Raycast(transform.position, directionToNewTarget, obstacleDetectionDistance))
            {
                targetPosition = newTarget;
                targetIsValid = true;
                Debug.DrawLine(transform.position, targetPosition, Color.green, 2f); // Debug
            }
            attempts++;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(initialPosition, patrolRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(targetPosition, 0.3f);
    }
}