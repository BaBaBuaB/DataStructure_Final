using UnityEngine;

public class RangeEnemies : Enemies
{
    [Header("Rotation Settings")]
    [SerializeField] private float shootPointDistance = 0.5f;

    [SerializeField] private Transform shootPoint;

    private void Awake()
    {
        InitializeComponents();
        // ตั้งค่าสเตตเริ่มต้นของ Enemy
        Initialized(20, 10, 500, 7, 6.5f, 1.1f, "Range_Enemy");

        detectRange = (int)chaseRange;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateBehavior();
    }

    protected override void UpdateBehavior()
    {
        base.UpdateBehavior();

        if (targetTransform != null && shootPoint != null)
        {
            UpdateShootPointPosition();
        }
    }

    protected override void AttackTarget()
    {
        if (targetTransform == null || attackTimer > 0) return;

        float distance = GetDistanceToTarget();
        if (distance <= attackRange)
        {
            attackTimer = nextAttackTime;

            // ยิงกระสุน
            if (shootPoint != null)
            {
                var bullet = ObjectPool.instance.Spawn("Bullet_Enemy");
                Bullet o = bullet.GetComponent<Bullet>();

                o.Attack = Attack;
                o.ownerBullet = "Bullet_Enemy";
                bullet.transform.SetPositionAndRotation(shootPoint.transform.position, shootPoint.transform.rotation);

            }
        }
    }

    private void UpdateShootPointPosition()
    {
        // คำนวณทิศทางจาก Pet ไปยัง Target
        Vector2 direction = ((Vector2)targetTransform.position - (Vector2)transform.position).normalized;

        // คำนวณมุมจากทิศทาง (ในหน่วย Radian แล้วแปลงเป็น Degree)
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // ตั้งค่าการหมุนของ shootPoint ให้หันไปทาง Target
        shootPoint.rotation = Quaternion.Euler(0f, 0f, angle);

        // คำนวณตำแหน่งของ shootPoint ให้อยู่ระหว่าง Pet และ Target
        // โดยห่างจาก Pet ด้วยระยะ shootPointDistance
        Vector2 offset = direction * shootPointDistance;
        shootPoint.position = (Vector2)transform.position + offset;
    }

    private void OnEnable()
    {
        Attack = Attack * StatusController.Instance.CurrentStats.enemyDamageBuff;
        maxHealth = maxHealth * StatusController.Instance.CurrentStats.enemyHealthBuff;
        Health = maxHealth;
    }
    */

    private void OnDisable()
    {
        maxHealth = 100;
        Attack = baseAttack;
        roomManager = null;
    }
}
