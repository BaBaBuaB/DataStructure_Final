using UnityEngine;

public class MeleeEnemies : Enemies
{
    private void Awake()
    {
        InitializeComponents();
        // ตั้งค่าสเตตเริ่มต้นของ Enemy
        Initialized(100, 10, 500, 4, 3f, 0.5f);

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
}
