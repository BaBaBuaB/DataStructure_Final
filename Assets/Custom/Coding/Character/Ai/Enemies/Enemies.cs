using UnityEngine;

public class Enemies : BaseAi, IDamageable
{
    #region "Enemy Parameters"
    [Header("Enemy Stats")]
    private float health;
    [SerializeField]protected float maxHealth;
    public float Health
    {
        get { return health; }
        set { health = Mathf.Clamp(value, 0, maxHealth); }
    }

    protected string tag = "";

    [SerializeField] protected LayerMask playerLayer;
    [SerializeField] protected LayerMask obstacleLayer;
    [SerializeField] public int enemyLevel;
    [SerializeField] public GameObject[] itemDrop;

    [Header("AI Behavior")]
    [SerializeField] protected float chaseRange = 15f;
    [SerializeField] protected float giveUpDistance = 25f;

    private bool isChasing = false;
    #endregion

    protected void Initialized(int hp, int atk, int spd, int detect, float atkRang, float atkCoolDown,string nameTag)
    {
        Health = hp;
        Attack = atk;
        Speed = spd;
        detectRange = detect;
        attackRange = atkRang;
        nextAttackTime = atkCoolDown;
        maxHealth = hp;
        tag = nameTag;
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
    public void TakeDamages(float damages)
    {
        if (IsDeath()) return;

        Health -= damages;
        Debug.Log($"{gameObject.name} รับดาเมจ {damages}! HP เหลือ: {Health}");

        if (Health <= 0)
        {
            DropItem();

            Attack = baseAttack;
            Health = maxHealth;

            ObjectPool.instance.Return(gameObject,tag);
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

    protected bool HasLineOfSight(Transform target)
    {
        if (target == null) return false;

        Vector2 direction = target.position - transform.position;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, direction.magnitude, obstacleLayer);
        return hit.collider == null;
    }
}
