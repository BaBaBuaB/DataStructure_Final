using System.Collections.Generic;
using UnityEngine;

public abstract class BaseAi : Identity
{
    #region"Parameter"
    #region "Movement & Pathfinding"
    [Header("Movement Settings")]
    [SerializeField] protected float stoppingDistance;
    [SerializeField] protected float pathUpdateInterval = 0.5f;

    [Header("Pathfinding Settings")]
    [SerializeField] protected float nodeSpacing;
    [SerializeField] protected float obstacleCheckRadius = 0.3f;
    [SerializeField] protected LayerMask obstacleLayer;
    [SerializeField] protected float maxPathfindingDistance = 50f;

    protected List<Vector2> currentPath;
    protected int currentPathIndex = 0;
    protected float pathUpdateTimer = 0f;
    protected Rigidbody2D rb;
    protected AStarPathFinder pathfinder;
    #endregion
    #region "Detection & Combat"
    [Header("Detection Settings")]
    [SerializeField] protected int detectRange;

    [Header("Combat Settings")]
    [SerializeField] protected float attackRange;
    [SerializeField] protected float nextAttackTime;

    protected Transform targetTransform;
    protected float attackTimer = 0f;
    #endregion
    #region "Obstacle Avoidance"
    [Header("Obstacle Avoidance")]
    [SerializeField] protected float avoidanceRadius = 1f;
    [SerializeField] protected float avoidanceStrength = 1f;
    [SerializeField] protected int raycastCount = 4;
    #endregion
    #endregion

    protected void InitializeComponents()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody2D>();
        }
        rb.gravityScale = 0f;
        rb.freezeRotation = true;
    }
    protected void InitializePathfinder()
    {
        pathfinder = new AStarPathFinder(nodeSpacing, obstacleCheckRadius, obstacleLayer);
    }

    protected virtual void UpdateBehavior()
    {
        if (attackTimer > 0)
        {
            attackTimer -= Time.deltaTime;
        }

        UpdateTarget();
        Behavior();
    }

    #region "Pathfinding Methods"
    protected void FindPath(Vector2 targetPos)
    {
        if (Vector2.Distance(transform.position, targetPos) > maxPathfindingDistance)
        {
            currentPath = null;
            rb.linearVelocity = Vector2.zero;
            OnTargetTooFar();
            return;
        }

        Vector2 startPos = transform.position;
        currentPath = pathfinder.FindPath(startPos, targetPos, stoppingDistance);
        currentPathIndex = 0;

        if (currentPath != null)
        {
            float pathDistance = AStarPathFinder.CalculatePathDistance(currentPath);
        }
        else
        {
            OnPathNotFound();
        }
    }
    public override void Move()
    {
        if (currentPath == null || currentPath.Count == 0)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        if (currentPathIndex >= currentPath.Count)
        {
            rb.linearVelocity = Vector2.zero;
            AttackTarget();
            return;
        }

        Vector2 targetPos = currentPath[currentPathIndex];
        Vector2 direction = (targetPos - (Vector2)transform.position).normalized;
        float distance = Vector2.Distance(transform.position, targetPos);

        if (distance <= stoppingDistance)
        {
            currentPathIndex++;
            if (currentPathIndex >= currentPath.Count)
            {
                rb.linearVelocity = Vector2.zero;
                AttackTarget();
                return;
            }
        }

        Vector2 avoidance = CalculateAvoidance();
        Vector2 finalDirection = (direction + avoidance).normalized;

        rb.linearVelocity = finalDirection * Speed;
    }

    protected virtual void ChaseTarget()
    {
        if (targetTransform == null) return;

        pathUpdateTimer += Time.deltaTime;
        if (pathUpdateTimer >= pathUpdateInterval)
        {
            pathUpdateTimer = 0f;
            FindPath(targetTransform.position);
        }

        Move();
    }
    #endregion

    #region "Detection & Utility Methods"
    protected float GetDistanceToTarget()
    {
        if (targetTransform == null) return float.MaxValue;
        return Vector2.Distance(transform.position, targetTransform.position);
    }
    protected bool IsWithinRange(float range)
    {
        return GetDistanceToTarget() <= range;
    }

    protected List<IDamageable> DetectObjectsInRadius<IDamageable>(float radius, LayerMask layer) where IDamageable : Component
    {
        List<IDamageable> detectedObjects = new List<IDamageable>();
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, radius, layer);

        foreach (Collider2D hit in hits)
        {
            if (hit.gameObject == gameObject) continue;

            IDamageable component = hit.GetComponent<IDamageable>();
            if (component != null)
            {
                Vector2 directionToTarget = (hit.transform.position - transform.position).normalized;
                float angleToTarget = Vector2.Angle(transform.up, directionToTarget);

                detectedObjects.Add(component);
            }
        }

        return detectedObjects;
    }

    protected bool HasLineOfSight(Transform target)
    {
        if (target == null) return false;

        Vector2 direction = target.position - transform.position;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, direction.magnitude, obstacleLayer);
        return hit.collider == null;
    }

    protected Vector2 CalculateAvoidance()
    {
        Vector2 avoidanceForce = Vector2.zero;

        for (int i = 0; i < raycastCount; i++)
        {
            float angle = (360f / raycastCount) * i;
            Vector2 direction = Quaternion.Euler(0, 0, angle) * Vector2.up;

            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, avoidanceRadius, obstacleLayer);

            if (hit.collider != null)
            {
                float distance = hit.distance;
                float strength = 1f - (distance / avoidanceRadius);
                Vector2 pushDirection = ((Vector2)transform.position - hit.point).normalized;
                avoidanceForce += pushDirection * strength * avoidanceStrength;
            }
        }

        return avoidanceForce;
    }
    #endregion

    #region "Abstract & Virtual Methods"
    protected abstract void Behavior();

    protected abstract void UpdateTarget();

    protected virtual void AttackTarget() { }
    protected virtual void OnPathNotFound()
    {
        //Debug.LogWarning($"{gameObject.name}: ไม่พบเส้นทาง!");
    }
    protected virtual void OnTargetTooFar() { }
    #endregion

}
