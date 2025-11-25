using UnityEngine;

public class Bullet : Identity
{
    public string ownerBullet = "";
    public float reflectionDamping = 1f; // ลดความเร็วเมื่อสะท้อน (1.0 = ไม่ลด)
    private Vector2 currentDirection;

    private void Start()
    {
        Speed = 3;
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Move();
    }

    public override void Move()
    {
        transform.Translate(Vector2.right *Time.deltaTime* Speed);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Barriar") && ownerBullet == "Bullet_Enemy")
        {
            Debug.Log("Barriar");
            return;
        }

        if (collision.gameObject.TryGetComponent<Enemies>(out Enemies enemies) && (ownerBullet == "Player" || ownerBullet == "Bullet_Pet"))
        {
            enemies.TakeDamages(Attack);

            if (ownerBullet == "Player")
            {
                ownerBullet = "Bullet_Enemy";
                ObjectPool.instance.Return(gameObject, ownerBullet);
            }
            else
            {
                ObjectPool.instance.Return(gameObject, ownerBullet);
            }
        }
        else if (collision.gameObject.CompareTag("Player") && ownerBullet == "Bullet_Enemy")
        {
            var player = collision.gameObject.GetComponent<Player>();

            player.TakeDamages(Attack);

            ObjectPool.instance.Return(gameObject, ownerBullet);
        }

        ObjectPool.instance.Return(gameObject, ownerBullet);
    }
}
