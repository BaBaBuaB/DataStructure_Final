using UnityEngine;

public class HealPets : Pet
{
    private  void Awake()
    {
        InitializeComponents();
        // ตั้งค่าสเตตเริ่มต้นของ Enemy
        Initialized(10, 500, 20, 6f, 0.5f);

        // หาเจ้าของ (Player)
        if (owner == null)
        {
            GameObject player = GameObject.Find("Player");
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

    protected override void AttackTarget()
    {
        if (owner.IsDeath() || attackTimer > 0) return;

        float distance = GetDistanceToTarget();
        if (distance <= attackRange)
        {
            attackTimer = nextAttackTime;
            owner.Health += Attack;
            Debug.Log($"{gameObject.name}: รักษา {owner.Health}");
        }
    }

    protected override void Behavior()
    {
        if (owner == null) return;

        FollowOwner();

        if (owner.Health < 90)
        {
            AttackTarget();
        }
    }

    protected override void UpdateTarget()
    {
        //base.UpdateTarget();
        targetTransform = owner.GetTransform();
    }
}
