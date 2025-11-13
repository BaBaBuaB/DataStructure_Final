using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public class Pet : BaseAi
{
    #region "Pet Parameters"
    [Header("Follow Settings")]
    [SerializeField] protected Transform owner; // ผู้เล่น
    [SerializeField] protected float followDistance = 3f; // ระยะห่างที่ติดตาม
    [SerializeField] protected float stopDistance; // ระยะที่หยุด

    [Header("Combat Settings")]
    [SerializeField] protected LayerMask enemyLayer;
    [SerializeField] protected float protectDistance;

    private Enemies currentTarget;
    private bool isProtecting = false;
    #endregion

    protected void Initialized(int atk, int spd,int detect, float atkRang, float atkCoolDown)
    {
        Attack = atk;
        Speed = spd;
        detectRange = detect;
        attackRange = atkRang;
        nextAttackTime = atkCoolDown;
    }

    #region"Abstract Class"

    protected override void UpdateTarget()
    {
        // ตรวจสอบศัตรูใกล้เคียง
        CheckForThreats();

        if (isProtecting && currentTarget != null && !currentTarget.IsDeath())
        {
            // ไล่ตามศัตรู
            targetTransform = currentTarget.GetTransform();
            stoppingDistance = attackRange - 1f;

            // ตรวจสอบระยะทาง ถ้าไกลเกินไปให้กลับไปหาเจ้าของ
            float distanceToEnemy = Vector2.Distance(transform.position, currentTarget.GetTransform().position);
            if (distanceToEnemy > protectDistance * 2f)
            {
                isProtecting = false;
                currentTarget = null;
            }
        }
        else
        {
            // ไม่มีศัตรู กลับไปติดตามเจ้าของ
            isProtecting = false;
            currentTarget = null;
            targetTransform = owner;
            stoppingDistance = stopDistance;
        }
    }

    protected override void Behavior()
    {
        if (owner == null) return;

        if (isProtecting)
        {
            // โหมดป้องกัน - ไล่ตามและโจมตีศัตรู
            ChaseTarget();
        }
        else
        {
            // โหมดติดตาม - ติดตามเจ้าของ
            FollowOwner();
        }
    }

    #endregion

    #region "Follow Owner"
    private void FollowOwner()
    {
        if (owner == null) return;

        float distanceToOwner = Vector2.Distance(transform.position, owner.position);

        // ติดตามเจ้าของถ้าอยู่ไกลเกินไป
        if (distanceToOwner > followDistance)
        {
            pathUpdateTimer += Time.deltaTime;
            if (pathUpdateTimer >= pathUpdateInterval)
            {
                pathUpdateTimer = 0f;
                FindPath(owner.position);
            }
            Move();
        }
        else
        {
            // อยู่ใกล้เจ้าของพอแล้ว หยุดเคลื่อนที่
            rb.linearVelocity = Vector2.zero;
        }
    }
    #endregion

    #region "Combat"
    private void CheckForThreats()
    {
        // ใช้ Collider2D แทน IDamageable
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, protectDistance, enemyLayer);

        if (hits.Length > 0 && owner != null)
        {
            Enemies closestToOwner = null;
            float closestDistance = float.MaxValue;

            foreach (Collider2D hit in hits)
            {
                if (hit.gameObject == gameObject) continue;

                Enemies damageable = hit.GetComponent<Enemies>();
                if (damageable != null && !damageable.IsDeath())
                {
                    float distanceToOwner = Vector2.Distance(hit.transform.position, owner.position);
                    if (distanceToOwner < closestDistance)
                    {
                        closestDistance = distanceToOwner;
                        closestToOwner = damageable;
                    }
                }
            }

            if (closestToOwner != null)
            {
                //Debug.Log("Found Enemy!");
                currentTarget = closestToOwner;
                isProtecting = true;
            }
        }
    }

    protected override void AttackTarget()
    {
        if (currentTarget == null || currentTarget.IsDeath()) return;
        if (attackTimer > 0) return;

        float distance = Vector2.Distance(transform.position, currentTarget.GetTransform().position);
        if (distance <= attackRange)
        {
            attackTimer = nextAttackTime;
            currentTarget.TakeDamages(Attack);
            //Debug.Log($"{gameObject.name}: โจมตีศัตรู! ดาเมจ {Attack}");
        }
    }
    #endregion

    #region "Callbacks"

    protected override void OnTargetTooFar()
    {
        if (isProtecting)
        {
            //Debug.Log($"{gameObject.name}: ศัตรูอยู่ไกลเกินไป หยุดไล่ตาม");
            isProtecting = false;
            currentTarget = null;
        }
    }
    #endregion
}
