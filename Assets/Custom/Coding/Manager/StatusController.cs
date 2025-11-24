using System.Collections.Generic;
using UnityEngine;
using static GameManager;


public class StatusController : MonoBehaviour
{
    
    public static StatusController Instance { get; private set; }

    public int CurrentWorldTier { get; private set; } = 1;
    public GameManager.WorldTierStats CurrentStats { get; private set; }
    
    public Dictionary<int, WorldTierStats> tierStatsData;

    void Awake()
    {
        
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeTierData();
            
            //tier 1 ก่อนเลย
            ApplyCurrentTierSettings(true);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeTierData()
    {
        tierStatsData = new Dictionary<int, WorldTierStats>();

        
        tierStatsData[1] = new WorldTierStats {
            enemyHealthBuff = 1.0f,
            enemyDamageBuff = 1.0f,
            playerDamageNerf = 1.0f,
            extraMonsterCap = 0
        };

        
        tierStatsData[2] = new WorldTierStats {
            enemyHealthBuff = 2f,  
            enemyDamageBuff = 1.5f, 
            playerDamageNerf = 0.8f, 
            extraMonsterCap = 3      
        };

        
        tierStatsData[3] = new WorldTierStats {
            enemyHealthBuff = 3f,  
            enemyDamageBuff = 2f,  
            playerDamageNerf = 0.7f, 
            extraMonsterCap = 5      
        };
        
        
    }

    //GameManagerจะเรียกใช้
    public void IncreaseWorldTier()
    {
        int nextTier = CurrentWorldTier + 1;
        if (tierStatsData.ContainsKey(nextTier))
        {
            CurrentWorldTier = nextTier;
            ApplyCurrentTierSettings(false); //เรียกใช้ค่า Tier ใหม่
            Debug.Log($"World Tier Increased to: {CurrentWorldTier}");
        }
        else
        {
            CurrentWorldTier = nextTier;
            Debug.Log("Max World Tier reached!");
        }
    }

    
    private void ApplyCurrentTierSettings(bool isInitialization)
    {
        if (tierStatsData.TryGetValue(CurrentWorldTier, out GameManager.WorldTierStats settings))
        {
            CurrentStats = settings;
        }
    }
}
