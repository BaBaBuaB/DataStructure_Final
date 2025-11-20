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

    public void Reflex(string newOwner, Collision2D collision)
    {
        ownerBullet = newOwner;
        gameObject.transform.Rotate(0,0,-1);

        // ดึง ContactPoint แรก
        ContactPoint2D contact = collision.GetContact(0);

        // คำนวณทิศทางสะท้อน
        Vector2 reflectDirection = Vector2.Reflect(currentDirection, contact.normal);

        // อัพเดททิศทางปัจจุบัน
        currentDirection = reflectDirection.normalized;

        // ตั้งค่าความเร็วใหม่ (ลดลงตาม damping)
        float newSpeed = Speed * reflectionDamping;
        rb.linearVelocity = currentDirection * newSpeed;

        // หมุนกระสุนไปทางที่สะท้อน
        float angle = Mathf.Atan2(currentDirection.y, currentDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacles"))
        {
            ObjectPool.instance.Return(gameObject,ownerBullet);
        }
        else if(collision.gameObject.TryGetComponent<Enemies>(out Enemies enemies) && (ownerBullet == "Player" || ownerBullet == "Bullet_Pet"))
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
        else if (collision.gameObject.TryGetComponent<Player>(out Player player) && ownerBullet == "Bullet_Enemy")
        {
            player.TakeDamages(Attack);
            ObjectPool.instance.Return(gameObject, ownerBullet);
        }
        else if(collision.gameObject.name == "Barrir")
        {
            Reflex("Player",collision);
        }
    }
}
