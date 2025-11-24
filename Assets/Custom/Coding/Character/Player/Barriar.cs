using UnityEngine;

public class Barriar : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<Bullet>(out Bullet bullet))
        {
            if (bullet.ownerBullet == "Bullet_Enemy")
            {
                bullet.Reflect("Player", collision);
            }
        }
    }
}
