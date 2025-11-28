using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Enemies : BaseAi, IDamageable
{
    #region "Enemy Parameters"
    [Header("Enemy Stats")]
    private float health;
    private float maxHealth;
    public float MaxHealth
    {
        get { return maxHealth; }
        set { maxHealth = value; }
    }
    public float Health
    {
        get { return health; }
        set { health = Mathf.Clamp(value, 0, maxHealth); }
    }

    protected string tag = "";

    [SerializeField] protected LayerMask playerLayer;
    [SerializeField] protected LayerMask obstacleLayer;
    [SerializeField] public int enemyLevel;
    [SerializeField] public GameObject itemDrop;

    [Header("AI Behavior")]
    [SerializeField] protected float chaseRange = 15f;
    [SerializeField] protected float giveUpDistance = 25f;

    private bool isChasing = false;

    public RoomManager roomManager;
    #endregion

    protected void Initialized(int hp, int atk, int spd, int detect, float atkRang, float atkCoolDown,string nameTag)
    {
        maxHealth = hp;
        Health = hp;
        Attack = atk;
        Speed = spd;
        detectRange = detect;
        attackRange = atkRang;
        nextAttackTime = atkCoolDown;
        tag = nameTag;
    }
    #region "Item Drop"
    private void DropItem()
    {
        if (itemDrop == null ) return;


        if (itemDrop != null)
        {
            var item = ObjectPool.instance.Spawn(itemDrop.name);
            item.transform.SetLocalPositionAndRotation(gameObject.transform.position,gameObject.transform.rotation);
        }
    }

    private IEnumerator PlayDeathAnim()
    {
        animator.SetBool("Dead", true);
        yield return new WaitForSeconds(2f);
        DropItem();
        ObjectPool.instance.Return(gameObject, tag);
        Attack = baseAttack;
        Health = maxHealth;

    }
    #endregion

    #region "IDamageable Implementation"
    public void TakeDamages(float damages)
    {
        if (IsDeath()) return;

        animator.SetTrigger("Hit");
        Health -= damages;

        if (IsDeath())
        {

            if (roomManager != null)
            {
                roomManager.OnEnemiesDeath();
            }

            StartCoroutine(PlayDeathAnim());
        }
    }

    public bool IsDeath()
    {
        return Health <= 0;
    }

    public Transform GetTransform()
    {
        return transform;
    }
    #endregion

    #region "Abstract Implementation"
    protected override void UpdateBehavior()
    {
        if (IsDeath()) return;

        base.UpdateBehavior();
    }

    protected override void UpdateTarget()
    {
        if (targetTransform == null)
        {
            DetectPlayer();
        }

        if (targetTransform == null) return;

        float distanceToTarget = GetDistanceToTarget();

        // เลิกไล่ตามถ้าไกลเกินไป
        if (distanceToTarget > giveUpDistance)
        {
            isChasing = false;
            rb.linearVelocity = Vector2.zero;
            return;
        }

        // เริ่มไล่ตามถ้าอยู่ในระยะ
        isChasing = distanceToTarget <= chaseRange;

    }

    protected override void Behavior()
    {
        if(targetTransform == null) return;

        if (isChasing && HasLineOfSight(targetTransform))
        {
            float distanceToTarget = GetDistanceToTarget();

            // ถ้าอยู่ในระยะโจมตีให้หยุด
            if (distanceToTarget <= attackRange)
            {
                rb.linearVelocity = Vector2.zero;
                AttackTarget();
            }
            else
            {
                // ยังไม่ถึงระยะโจมตีให้ Chase ต่อ
                ChaseTarget();
            }
        }
    }
    #endregion

    #region "Detection"
    private void DetectPlayer()
    {
        // ตรวจจับผู้เล่น
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, detectRange, playerLayer);

        if (hits.Length > 0)
        {
            // หาผู้เล่นที่ใกล้ที่สุด
            Transform closestPlayer = null;
            float closestDistance = float.MaxValue;

            foreach (Collider2D hit in hits)
            {
                if (hit.gameObject == gameObject) continue;

                float distance = Vector2.Distance(transform.position, hit.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestPlayer = hit.transform;
                }
            }

            if (closestPlayer != null)
            {
                targetTransform = closestPlayer;
                isChasing = true;
            }
        }
    }
    #endregion

    #region "Combat"
    protected override void AttackTarget()
    {
        if (targetTransform == null || attackTimer > 0) return;

        Player damageable = targetTransform.GetComponent<Player>();
        if (damageable == null || damageable.IsDeath()) return;

        float distance = GetDistanceToTarget();
        if (distance <= attackRange)
        {
            attackTimer = nextAttackTime;

            damageable.TakeDamages(Attack);
        }
    }
    #endregion

    protected bool HasLineOfSight(Transform target)
    {
        if (target == null) return false;

        Vector2 direction = target.position - transform.position;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, direction.magnitude, obstacleLayer);
        return hit.collider == null;
    }


}
