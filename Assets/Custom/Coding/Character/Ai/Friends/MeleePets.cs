using UnityEngine;

public class MeleePets : Pet
{
    private  void Awake()
    {
        InitializeComponents();
        Initialized(10, 600, 4, 2f, 0.5f);

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
