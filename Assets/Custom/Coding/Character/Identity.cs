using UnityEngine;

public abstract class Identity : MonoBehaviour
{
    private int speed;
    private int attack;
    protected Rigidbody2D rb;

    public int Speed
    { 
        get { return speed; }
        set { speed = value; }
    }
    public int Attack
    { 
        get { return attack; } 
        set { attack = value; } 
    }

    public abstract void Move();
}
