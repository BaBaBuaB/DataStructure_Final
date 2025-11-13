using UnityEngine;

public class HealPets : Pet
{
    private  void Awake()
    {
        InitializeComponents();
        InitializePathfinder();

        // ตั้งค่าสเตตเริ่มต้นของ Enemy
        Initialized(10, 3, 20, 3f, 0.5f);

        // หาเจ้าของ (Player)
        if (owner == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                owner = player.transform;
            }
        }

        stoppingDistance = stopDistance;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateBehavior();
    }
}
