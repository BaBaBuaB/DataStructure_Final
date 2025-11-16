using UnityEngine;

public class RangeEnemies : Enemies
{
    [SerializeField] private Transform shootPoint;
    [SerializeField] private GameObject bullet;

    private void Awake()
    {
        InitializeComponents();
        // ตั้งค่าสเตตเริ่มต้นของ Enemy
        Initialized(100, 10, 500, 30, 10f, 0.5f);

        // หา Player เป็นเป้าหมายเริ่มต้น
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            targetTransform = player.transform;
        }

        detectRange = (int)chaseRange;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateBehavior();
    }

    protected override void AttackTarget()
    {
        if (targetTransform == null || attackTimer > 0) return;

        float distance = GetDistanceToTarget();
        if (distance <= attackRange)
        {
            attackTimer = nextAttackTime;
            rb.linearVelocity = Vector2.zero;

            //Instantiate();
        }
    }
}
