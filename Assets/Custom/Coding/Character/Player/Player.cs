using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Identity, IDamageable
{
    #region"Parameter"
    #region"Health"
    private float health;
    public float maxHealth = 100;
    public float Health
    { 
        get { return health; }
        set { health = Mathf.Clamp(value,0,maxHealth); }
    }
    #endregion
    public int keyCount;
    public int poolsPet;
    public int currentPet = 0;
    public List<Pet> pets;

    #region "Input"
    private PlayerInput playerInput;
    private InputActionMap inputActionsMap;
    private InputAction moveAction;
    private InputAction blockAction;
    private InputAction closeGuide;
    #endregion

    [SerializeField]private float coolDown = 5;
    private float timerCoolDown;
    private float duration = 2;
    public GameObject barriar;
    public bool barriarActive = false;

    [SerializeField]private Animator animator;
    [SerializeField] private GameObject spriteObj;
    #endregion
    private void Initialized(int hp, int atk, int spd)
    {
        maxHealth = hp;
        Health = hp;
        Attack = atk;
        Speed = spd;
    }

    private void Awake()
    {
        Initialized(100,10,700);
        UIManager.instance.UpdateHealth(Health,maxHealth);

        rb = GetComponent<Rigidbody2D>();
        playerInput = GetComponent<PlayerInput>();
        inputActionsMap = playerInput.actions.FindActionMap("Controller");
        moveAction = inputActionsMap.FindAction("Move");
        blockAction = inputActionsMap.FindAction("BlockBullet");
        closeGuide = inputActionsMap.FindAction("CloseAction");

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
        CloseUi();

        if (timerCoolDown  >= 0)
        {
            timerCoolDown -= Time.deltaTime;
        }
    }

    #region"InterfaceIDamages"
    public void TakeDamages(float damages)
    {
        if (barriarActive || IsDeath())
        {
            return;
        }

        Health -= damages;

        UpdateHealth();
        Debug.Log(Health);

        if (IsDeath())
        {
            animator.SetBool("Death",true);
            UIManager.instance.CallGameOverUi();
        }
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
            case "Attack_Potion": Attack += items.valueItem;
                SetStatPet();
                break;
            case "Health_Potion": Health += items.valueItem;
                maxHealth += items.valueItem/2;
                UpdateHealth();
                break;
            case "Key": keyCount += items.valueItem;
                break;
            default: Debug.Log("NothingHappen");
                break;
        }

        ObjectPool.instance.Return(items.gameObject,items.nameItem);
    }

    public override void Move()
    {
        Vector2 move = moveAction.ReadValue<Vector2>();
        if (moveAction.inProgress) 
        {
            animator.SetBool("Run",true); 
        }
        else
        {
            animator.SetBool("Run", false);
        }
        Vector2 moveDir = move * Speed * Time.deltaTime;

        if (move.x > 0)
        {
            spriteObj.transform.localScale = new Vector3(1, 1, 1);
        }
        else if (move.x < 0)
        {
            spriteObj.transform.localScale = new Vector3(-1, 1, 1);
        }
        rb.linearVelocity = moveDir;
    }
    private void Block()
    {
        if (blockAction.triggered && timerCoolDown <= 0 && !barriarActive)
        {
            barriarActive= true;
            StartCoroutine(ActivateBarrier());
        }
    }
    private void CloseUi()
    {
        if (closeGuide.IsPressed())
        {
            UIManager.instance.CallSlimeStatesUi();
        }
        else
        {
            UIManager.instance.CloseSlimeStatesUi();
        }
    }

    private IEnumerator ActivateBarrier()
    {
        barriar.SetActive(true);
        yield return new WaitForSeconds(duration);
        timerCoolDown = coolDown;
        barriar.SetActive(false);
        barriarActive = false;
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
    private void OnDisable()
    {
        inputActionsMap.Disable();
    }

    private void OnEnable()
    {
        inputActionsMap.Enable();
    }
    #endregion

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision == null)
        {
            return;
        }

        if (collision.gameObject.CompareTag("Items"))
        {
            Items item = collision.gameObject.GetComponent<Items>();

            if (item != null)
            {
                GetItems(item);
            }
        }
        else if(collision.gameObject.CompareTag("Door"))
        {
            if (keyCount > 0)
            {
                collision.gameObject.SetActive(false);
            }
        }
    }

    public void SummonPets()
    {
        if (pets.Count >= poolsPet) return;

        string petTag = "";

        if (pets.Count <= 0 || currentPet >= 3) currentPet = 0;
        else { currentPet++; }

        if (currentPet == 0) petTag = "Melee_Pet";
        else if (currentPet == 1) petTag = "Range_Pet";
        else if (currentPet == 2) petTag = "Heal_Pet";
        else if (currentPet == 3) petTag = "Devour_Pet";

        var newPet = ObjectPool.instance.Spawn(petTag);

        newPet.transform.SetPositionAndRotation(gameObject.transform.position, gameObject.transform.rotation);

        pets.Add(newPet.GetComponent<Pet>());
    }

    public void UpdateHealth()
    {
        UIManager.instance.UpdateHealth(Health, maxHealth);
    }
}
