using UnityEngine;
using System;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public struct WorldTierStats
    {
        public float enemyHealthBuff, enemyDamageBuff, playerDamageNerf;
        public int extraMonsterCap;
        public int worldTier;
        public int extraMon;
    }
    private static GameManager instance;
    [SerializeField]private UIManager UIManager;
    

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            //DontDestroyOnLoad(gameObject); // (ถ้า GameManager ต้องอยู่ตลอด)
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public static GameManager GetInstance()
    {
        return instance;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        worldTier = 1;
        extraMon = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CheckCompleteState()
    {
       
        bool isStateComplete = true; //สมมติว่าเช็คแล้วว่าผ่าน

        if (isStateComplete)
        {
            worldTier++;
            if (StatusController.Instance != null)
            {
                StatusController.Instance.IncreaseWorldTier();
            }
            
        }
    }
}
