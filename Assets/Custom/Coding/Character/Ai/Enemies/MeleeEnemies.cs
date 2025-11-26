using UnityEngine;

public class MeleeEnemies : Enemies
{
    private void Awake()
    {
        InitializeComponents();
        // ตั้งค่าสเตตเริ่มต้นของ Enemy
        Initialized(200, 10, 500, 4, 1.5f, 1.5f, "Melee_Enemy");

        detectRange = (int)chaseRange;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateBehavior();
    }

    
    private void OnEnable()
    {
        Attack = Attack * StatusController.Instance.CurrentStats.enemyDamageBuff;
        MaxHealth = MaxHealth * StatusController.Instance.CurrentStats.enemyHealthBuff;
        Health = MaxHealth;
    }

    private void OnDisable()
    {
        MaxHealth = 100;
        Attack = baseAttack;
        roomManager = null;
    }
    
}
