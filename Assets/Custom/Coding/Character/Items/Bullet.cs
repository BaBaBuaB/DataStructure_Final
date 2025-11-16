using UnityEngine;

public class Bullet : Identity
{
    public string ownerBullet = "";

    private void Start()
    {
        Speed = 3;
    }

    private void Update()
    {
        Move();
    }

    public override void Move()
    {
        transform.Translate(Vector2.right *Time.deltaTime* Speed);
    }

    public void Reflex(string newOwner)
    {
        ownerBullet = newOwner;
        gameObject.transform.Rotate(0,0,-1);
    }
}
