using UnityEngine;

public class MeleeEnemies : Enemies
{
    private void Awake()
    {
        InitializeComponents();
        InitializePathfinder();

        // ตั้งค่าสเตตเริ่มต้นของ Enemy
        Initialized(100, 10, 8, 30, 2f, 0.3f);

        // หา Player เป็นเป้าหมายเริ่มต้น
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            targetTransform = player.transform;
        }

        detectRange = (int)chaseRange - 1;
        stoppingDistance = attackRange - 0.5f;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateBehavior();
    }
}
