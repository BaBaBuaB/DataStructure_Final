using UnityEngine;

public class Enemies : BaseAi, IDamageable
{
    #region "Enemy Parameters"
    [Header("Enemy Stats")]
    private int health;
    public int Health
    {
        get { return health; }
        set { health = value; }
    }

    [SerializeField] protected LayerMask playerLayer;
    [SerializeField] public int enemyLevel;
    [SerializeField] public GameObject[] itemDrop;

    [Header("AI Behavior")]
    [SerializeField] protected float chaseRange = 15f;
    [SerializeField] protected float giveUpDistance = 25f;

    private bool isChasing = false;
    #endregion

    protected void Initialized(int hp, int atk, int spd, int detect, float atkRang, float atkCoolDown)
    {
        Health = hp;
        Attack = atk;
        Speed = spd;
        detectRange = detect;
        attackRange = atkRang;
        nextAttackTime = atkCoolDown;
    }
    #region "Item Drop"
    private void DropItem()
    {
        if (itemDrop == null || itemDrop.Length == 0) return;

        // สุ่มดรอปไอเทม
        int randomIndex = Random.Range(0, itemDrop.Length);
        GameObject item = itemDrop[randomIndex];

        if (item != null)
        {
            Instantiate(item, transform.position, Quaternion.identity);
            Debug.Log($"{gameObject.name} ดรอปไอเทม: {item.name}");
        }
    }
    #endregion

    #region "IDamageable Implementation"
    public void TakeDamages(int damages)
    {
        if (IsDeath()) return;

        Health -= damages;
        Debug.Log($"{gameObject.name} รับดาเมจ {damages}! HP เหลือ: {Health}");

        if (Health <= 0)
        {
            DropItem();
            Destroy(gameObject);
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
        if (targetTransform == null) return;

        if (isChasing && HasLineOfSight(targetTransform))
        {
            ChaseTarget();
        }
        else if (!isChasing)
        {
            rb.linearVelocity = Vector2.zero;
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

        float distance = GetDistanceToTarget();
        if (distance <= attackRange)
        {
            attackTimer = nextAttackTime;

            Player damageable = targetTransform.GetComponent<Player>();
            if (damageable != null)
            {
                damageable.TakeDamages(Attack);
                Debug.Log($"{gameObject.name}: โจมตีเป้าหมาย! ดาเมจ {Attack}");
            }
        }
    }
    #endregion

    #region "Callbacks"

    protected override void OnPathNotFound()
    {
        base.OnPathNotFound();
        isChasing = false;
    }

    protected override void OnTargetTooFar()
    {
        //Debug.Log($"{gameObject.name}: เป้าหมายอยู่ไกลเกินไป หยุดไล่ตาม");
        isChasing = false;
        rb.linearVelocity = Vector2.zero;
    }
    #endregion

    #region "Target Switching"
    public void SetTarget(Transform newTarget)
    {
        if (newTarget == null) return;

        float distance = Vector2.Distance(transform.position, newTarget.position);
        if (distance <= chaseRange)
        {
            targetTransform = newTarget;
            isChasing = true;
            //Debug.Log($"{gameObject.name}: เปลี่ยนเป้าหมายใหม่!");
        }
    }
    #endregion
}
