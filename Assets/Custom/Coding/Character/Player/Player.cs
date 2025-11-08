using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class Player : Identity, IDamageable
{
    private int health;
    public int Health
    { 
        get { return health; }
        set { health = value; }
    }

    public int countItems;
    public int poolsPet;
    public List<Pet> pets;

    public InputActionMap inputActionsMap;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
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

    public void GetItems(Items items)
    {

    }

    public void BlockBullet()
    {
        
    }
}
