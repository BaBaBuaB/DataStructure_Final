using UnityEngine;

public class DevourPets : Pet
{
    private void Awake()
    {
        InitializeComponents();
        // ตั้งค่าสเตตเริ่มต้นของ Enemy
        Initialized(10, 150, 4, 2f, 2.5f);

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
    private void FixedUpdate()
    {
        UpdateBehavior();
    }

}
