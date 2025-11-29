using UnityEngine;

public class HealPets : Pet
{
    private  void Awake()
    {
        InitializeComponents();
        // ตั้งค่าสเตตเริ่มต้นของ Enemy
        Initialized(10, 200, 20, 6f, 4f);

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

    private void FixedUpdate()
    {
        UpdateBehavior();
    }

    protected override void AttackTarget()
    {
        if (owner.IsDeath() || attackTimer > 0) return;

        float distance = GetDistanceToTarget();
        if (distance <= attackRange)
        {
            animator.SetTrigger("attack");
            attackTimer = nextAttackTime;
            owner.Health += Attack;
            owner.UpdateHealth();
        }
    }

    protected override void Behavior()
    {
        if (owner == null) return;

        FollowOwner();

        if (owner.Health <= owner.MaxHealth * 0.8)
        {
            AttackTarget();
        }
    }

    protected override void UpdateTarget()
    {
        targetTransform = owner.GetTransform();
    }
}
