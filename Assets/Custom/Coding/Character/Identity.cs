using UnityEngine;

public abstract class Identity : MonoBehaviour
{
    private int speed;
    private float attack;
    protected float baseAttack;
    protected Rigidbody2D rb;

    public int Speed
    { 
        get { return speed; }
        set { speed = value; }
    }
    public float Attack
    { 
        get { return attack; } 
        set { attack = Mathf.Clamp(value,baseAttack,1000); } 
    }

    public abstract void Move();
}
