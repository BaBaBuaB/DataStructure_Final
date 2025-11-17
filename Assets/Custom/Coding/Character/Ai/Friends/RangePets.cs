using UnityEngine;

public class RangePets : Pet
{
    [Header("Rotation Settings")]
    [SerializeField] private float shootPointDistance = 0.5f;

    [SerializeField] private Transform shootPoint;
    [SerializeField] private GameObject bullet;

    private void Awake()
    {
        InitializeComponents();
        // ตั้งค่าสเตตเริ่มต้นของ Enemy
        Initialized(10, 500, 12, 10f, 1.1f);

        // หาเจ้าของ (Player)
        if (owner == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                owner = player.GetComponent<Player>();
            }
        }
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

        // ตรวจสอบว่าเป้าหมายยังมีชีวิตอยู่
        IDamageable damageable = targetTransform.GetComponent<IDamageable>();
        if (damageable == null || damageable.IsDeath()) return;

        float distance = GetDistanceToTarget();
        if (distance <= attackRange)
        {
            attackTimer = nextAttackTime;
            // ยิงกระสุน
            if (bullet != null && shootPoint != null)
            {
                GameObject projectile = Instantiate(bullet, shootPoint.position, shootPoint.rotation);
                // TODO: ตั้งค่า damage และ target ให้กระสุน
                Debug.Log($"{gameObject.name}: ยิงกระสุนใส่ศัตรู!");
            }
        }
    }

    private void UpdateShootPointPosition()
    {
        Vector2 direction = ((Vector2)targetTransform.position - (Vector2)transform.position).normalized;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        shootPoint.rotation = Quaternion.Euler(0f, 0f, angle);

        Vector2 offset = direction * shootPointDistance;
        shootPoint.position = (Vector2)transform.position + offset;
    }
}
