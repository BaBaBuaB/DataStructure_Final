using UnityEngine;

public class MeleePets : Pet
{
    private  void Awake()
    {
        InitializeComponents();
        Initialized(10, 350, 4, 2f, 1.5f);

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
}
