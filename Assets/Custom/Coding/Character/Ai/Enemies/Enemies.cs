using UnityEngine;

public class Enemies : BaseAi, IDamageable
{
    private int health;
    public int Health
    {
        get { return health; }
        set { health = value; }
    }

    public int enemyLevel;
    public GameObject[] itemDrop;

    private void DropItem()
    {
        
    }

    #region"Interface"
    public void TakeDamages()
    {

    }

    public void IsDeath()
    {

    }
    #endregion
}
