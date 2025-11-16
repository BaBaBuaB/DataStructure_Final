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

    private Rigidbody2D rigidbody2;
    private PlayerInput playerInput;
    private InputActionMap inputActionsMap;
    private InputAction moveAction;
    private InputAction blockAction;

    [SerializeField]private float coolDown = 3;
    private float timerCoolDown;

    private void Initialized(int hp, int atk, int spd)
    {
        Health = hp;
        Attack = atk;
        Speed = spd;
    }

    private void Awake()
    {
        Initialized(100,10,10);

        rigidbody2 = GetComponent<Rigidbody2D>();
        playerInput = GetComponent<PlayerInput>();
        inputActionsMap = playerInput.actions.FindActionMap("Controller");
        moveAction = inputActionsMap.FindAction("Move");
        blockAction = inputActionsMap.FindAction("BlockBullet");
    }

    // Update is called once per frame
    void Update()
    {
        if (IsDeath())
        {
            return;
        }

        Move();


        timerCoolDown -= 1;
    }

    #region"InterfaceIDamages"
    public void TakeDamages(int damages)
    {
        if (IsDeath()) return;
        
        Health -= damages;
        Debug.Log($"{Health}");
    }

    public bool IsDeath()
    {
        
        return Health <= 0;
    }

    public Transform GetTransform()
    {
        return transform;
    }
    #endregion

    public void GetItems(Items items)
    {
        switch(items.nameItem)
        {
            case "AttackPotion": Attack += items.valueItem;
                SetStatPet();
                break;
            case "HealPotion": Health += items.valueItem;
                break;
            case "Key": countItems += items.valueItem;
                break;
            default: Debug.Log("NothingHappen");
                break;
        }

        Destroy(items.gameObject);
    }

    public override void Move()
    {
        Vector2 move = moveAction.ReadValue<Vector2>();
        Vector2 moveDir = move * Speed * Time.deltaTime;
        transform.Translate(moveDir);
    }

    public void SetStatPet()
    {
        if (pets.Count == 0 || pets.Count == null) return;

        foreach (var pet in pets)
        {
            pet.Attack = Attack;
        }
    }


    #region"Enable and Disable"
    private void OnEnable()
    {
        inputActionsMap.Enable();
    }

    private void OnDisable()
    {
        inputActionsMap.Disable();
    }
    #endregion

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision == null)
        {
            return;
        }

        else if (collision.gameObject.CompareTag("Items"))
        {
            Items item = collision.gameObject.GetComponent<Items>();

            if (item != null)
            {
                GetItems(item);
            }
        }
        else if(collision.gameObject.CompareTag("Bullet"))
        {
            Bullet bullet = collision.gameObject.GetComponent<Bullet>();

            if (blockAction.triggered && timerCoolDown <= 0 && bullet.ownerBullet == "Enemy")
            {
                bullet.Reflex("Player");
                timerCoolDown = coolDown;
                Debug.Log("Reflex");
            }
            else if (bullet.ownerBullet == "Enemy")
            {
                TakeDamages(bullet.Attack);
            }
        }
    }
}
