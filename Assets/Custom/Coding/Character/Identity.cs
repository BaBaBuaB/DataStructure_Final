using UnityEngine;

public class Identity : MonoBehaviour
{
    private int speed;
    private int attack;
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

    public void Move()
    {
        
    }
}
