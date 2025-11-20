using UnityEngine;

public class MeleeEnemies : Enemies
{
    private void Awake()
    {
        InitializeComponents();
        // ตั้งค่าสเตตเริ่มต้นของ Enemy
        Initialized(30, 10, 500, 4, 1.5f, 0.5f, "Melee_Enemy");

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
        maxHealth = maxHealth * StatusController.Instance.CurrentStats.enemyHealthBuff;
        Health = maxHealth;
    }

    private void OnDisable()
    {
        maxHealth = 100;
        Attack = baseAttack;
        roomManager = null;
    }
    
}
