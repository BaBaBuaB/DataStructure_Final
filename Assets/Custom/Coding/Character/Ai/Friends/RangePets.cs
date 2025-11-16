using UnityEngine;

public class RangePets : Pet
{
    [SerializeField] private Transform shootPoint;
    [SerializeField] private GameObject bullet;

    private void Awake()
    {
        InitializeComponents();
        // ตั้งค่าสเตตเริ่มต้นของ Enemy
        Initialized(10, 500, 20, 10f, 0.5f);

        // หาเจ้าของ (Player)
        if (owner == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
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

    protected override void AttackTarget()
    {
        base.AttackTarget();
    }
}
