using System.Collections.Generic;
using UnityEngine;

public class Pet : BaseAi
{
    #region "Pet Parameters"
    [Header("Follow Settings")]
    [SerializeField] protected Player owner; // ผู้เล่น
    [SerializeField] protected float followDistance = 2f; // ระยะห่างที่ติดตาม

    [Header("Combat Settings")]
    [SerializeField] protected LayerMask enemyLayer;
    private bool isProtecting = false;

    // สร้าง buffer ไว้ล่วงหน้า (ครั้งเดียว)
    private Collider2D[] hitBuffer; // ขนาด 20 ก็พอ
    private float threatCheckInterval = 0.2f;
    private float threatCheckTimer = 0f;

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

        if (isProtecting && targetTransform != null)
        {
            // ตรวจสอบว่าศัตรูยังมีชีวิตอยู่หรือไม่
            IDamageable damageable = targetTransform.GetComponent<IDamageable>();
            if (damageable != null && damageable.IsDeath())
            {
                // ศัตรูตายแล้ว
                isProtecting = false;
                targetTransform = null;
                return;
            }

            // ตรวจสอบระยะทาง ถ้าไกลเกินไปให้กลับไปหาเจ้าของ
            float distanceToEnemy = GetDistanceToTarget();
            if (distanceToEnemy > detectRange * 1.2f)
            {
                isProtecting = false;
                targetTransform = null;
            }
        }
        else
        {
            // ไม่มีศัตรู กลับไปติดตามเจ้าของ
            isProtecting = false;
            targetTransform = owner.GetTransform();
        }
    }

    protected override void Behavior()
    {
        if (owner == null) return;

        if (isProtecting)
        {
            float distanceToEnemy = GetDistanceToTarget();

            // ถ้าอยู่ในระยะโจมตีให้หยุด
            if (distanceToEnemy <= attackRange)
            {
                rb.linearVelocity = Vector2.zero;
                AttackTarget();
            }
            else
            {
                // ยังไม่ถึงระยะโจมตีให้ Chase
                ChaseTarget();
            }
        }
        else
        {
            FollowOwner();
        }
    }

    #endregion

    #region "Follow Owner"
    protected void FollowOwner()
    {
        float distanceToOwner = Vector2.Distance(transform.position, owner.GetTransform().position);

        // ติดตามเจ้าของถ้าอยู่ไกลเกินไป
        if (distanceToOwner > followDistance)
        {
            pathUpdateTimer += Time.deltaTime;
            if (pathUpdateTimer >= pathUpdateInterval)
            {
                pathUpdateTimer = 0f;
                FindPath(owner.GetTransform().position);
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
        threatCheckTimer += Time.deltaTime;
        if (threatCheckTimer < threatCheckInterval) return;
        threatCheckTimer = 0f;

        hitBuffer = Physics2D.OverlapCircleAll(transform.position, detectRange, enemyLayer);

        if (hitBuffer.Length > 0 && owner != null)
        {
            Transform closestToOwner = null;
            float closestDistance = float.MaxValue;

            foreach (Collider2D hit in hitBuffer)
            {
                if (hit.gameObject == gameObject) continue;

                IDamageable damageable = hit.GetComponent<IDamageable>();
                if (damageable != null && !damageable.IsDeath())
                {
                    float distanceToOwner = Vector2.Distance(hit.transform.position, owner.GetTransform().position);
                    if (distanceToOwner < closestDistance)
                    {
                        closestDistance = distanceToOwner;
                        closestToOwner = hit.transform;
                    }
                }
            }

            if (closestToOwner != null)
            {
                //Debug.Log("Found Enemy!");
                targetTransform = closestToOwner;
                isProtecting = true;
            }
        }
    }

    protected override void AttackTarget()
    {
        if (targetTransform == null || attackTimer > 0) return;

        IDamageable damageable = targetTransform.GetComponent<IDamageable>();
        if (damageable == null || damageable.IsDeath()) return;

        float distance = GetDistanceToTarget();
        if (distance <= attackRange)
        {
            attackTimer = nextAttackTime;
            damageable.TakeDamages(Attack);
            //Debug.Log($"{gameObject.name}: โจมตีศัตรู! ดาเมจ {Attack}");
        }
    }
    #endregion
}
