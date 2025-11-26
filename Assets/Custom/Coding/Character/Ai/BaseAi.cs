using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public abstract class BaseAi : Identity
{
    #region"Parameter"
    #region "Movement & Pathfinding"
    [Header("Movement Settings")]
    [SerializeField] protected float pathUpdateInterval = 0.5f;
    [SerializeField] protected float pathUpdateTimer = 0f;

    protected Path path;
    Seeker seeker;
    protected bool reachDis;
    protected int currentWayPoint = 0;

    #endregion
    #region "Detection & Combat"
    [Header("Detection Settings")]
    [SerializeField] protected int detectRange;

    [Header("Combat Settings")]
    [SerializeField] protected float attackRange;
    [SerializeField] protected float nextAttackTime;

    protected Transform targetTransform;
    protected float nextWayPoint = 5f;
    protected float attackTimer = 0f;
    #endregion

    [SerializeField]protected Animator animator;
    [SerializeField]private GameObject spriteObj;

    #endregion

    protected void InitializeComponents()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody2D>();
        }
        if (seeker == null)
        {
            seeker = gameObject.AddComponent<Seeker>();
        }
        rb.gravityScale = 0f;
        rb.freezeRotation = true;
    }

    protected virtual void UpdateBehavior()
    {
        if (attackTimer > 0)
        {
            attackTimer -= Time.deltaTime;
            return;
        }

        UpdateTarget();
        Behavior();
    }

    #region "Pathfinding Methods"
    protected void FindPath(Vector2 targetPos)
    {
        if (seeker.IsDone())
            seeker.StartPath(rb.position, targetPos,OnPathComplete);
    }

    protected void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWayPoint = 0;
        }
    }
    public override void Move()
    {
        if (path == null) return;

        if (currentWayPoint >= path.vectorPath.Count)
        {
            reachDis = true;
            return;
        }
        else
        {
            reachDis = false;
        }

        Vector2 dir = ((Vector2)path.vectorPath[currentWayPoint] - rb.position).normalized;
        Vector2 moveSpeed = dir * Speed * Time.deltaTime;

        rb.linearVelocity = moveSpeed;
        
        if (moveSpeed.x > 0)
        {
            spriteObj.transform.localScale = new Vector3(1, 1, 1);
        }
        else if (moveSpeed.x < 0)
        {
            spriteObj.transform.localScale = new Vector3(-1, 1, 1);
        }
        

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWayPoint]);

        if (distance < nextWayPoint)
        {
            currentWayPoint++;
        }
    }

    protected void ChaseTarget()
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

    protected float GetDistanceToTarget()
    {
        if (targetTransform == null) return float.MaxValue;
        return Vector2.Distance(transform.position, targetTransform.position);
    }
    #endregion
    #region "Abstract & Virtual Methods"
    protected abstract void Behavior();

    protected abstract void UpdateTarget();

    protected virtual void AttackTarget() { }
    #endregion

}
