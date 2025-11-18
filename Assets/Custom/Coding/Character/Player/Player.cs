using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class Player : Identity, IDamageable
{
    private float health;
    [SerializeField]private float maxHealth = 100;
    public float Health
    { 
        get { return health; }
        set { health = Mathf.Clamp(value,0,maxHealth); }
    }

    public int countItems;
    public int poolsPet;
    public List<Pet> pets;

    private PlayerInput playerInput;
    private InputActionMap inputActionsMap;
    private InputAction moveAction;
    private InputAction blockAction;

    [SerializeField]private float coolDown = 3;
    private float timerCoolDown;
    public GameObject barriar;

    private void Initialized(int hp, int atk, int spd)
    {
        maxHealth = hp;
        Health = hp;
        Attack = atk;
        Speed = spd;
    }

    private void Awake()
    {
        Initialized(100,10,400);

        rb = GetComponent<Rigidbody2D>();
        playerInput = GetComponent<PlayerInput>();
        inputActionsMap = playerInput.actions.FindActionMap("Controller");
        moveAction = inputActionsMap.FindAction("Move");
        blockAction = inputActionsMap.FindAction("BlockBullet");

        SummonPets();
    }

    // Update is called once per frame
    void Update()
    {
        if (IsDeath())
        {
            return;
        }

        Move();
        Block();

        timerCoolDown -= 1;
    }

    #region"InterfaceIDamages"
    public void TakeDamages(float damages)
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
                maxHealth += items.valueItem/2;
                break;
            case "Key": countItems += items.valueItem;
                break;
            default: Debug.Log("NothingHappen");
                break;
        }

        ObjectPool.instance.Return(items.gameObject,"");
    }

    public override void Move()
    {
        Vector2 move = moveAction.ReadValue<Vector2>();
        Vector2 moveDir = move * Speed * Time.deltaTime;
        rb.linearVelocity = moveDir;
    }
    private void Block()
    {
        if (blockAction.triggered && timerCoolDown <= 0)
        {
            barriar.SetActive(true);
            timerCoolDown = coolDown;
        }
        else
        {
            barriar.SetActive(false);
        }
    }

    public void SetStatPet()
    {
        if (pets.Count == 0 || pets == null) return;

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
    }

    public void SummonPets()
    {
        if (pets.Count >= poolsPet) return;

        int random = 0;
        string petTag = "";

        if (pets.Count <= 0 || random >= 3) random = 0;
        else { random++; }

        if (random == 0) petTag = "Range_Pet";
        else if (random == 1) petTag = "Melee_Pet";
        else if (random == 2) petTag = "Heal_Pet";
        else if (random == 3) petTag = "Devour_Pet";

        var newPet = ObjectPool.instance.Spawn(petTag);

        newPet.transform.SetPositionAndRotation(gameObject.transform.position, gameObject.transform.rotation);

        pets.Add(newPet.GetComponent<Pet>());
    }
}
