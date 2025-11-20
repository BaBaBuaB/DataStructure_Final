using UnityEngine;

public class DevourPets : Pet
{
    private void Awake()
    {
        InitializeComponents();
        // ตั้งค่าสเตตเริ่มต้นของ Enemy
        Initialized(10, 700, 4, 4f, 0.5f);

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

}
