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
    }
    private static GameManager instance;
    [SerializeField]private UIManager UIManager;
    [SerializeField]private Player player;

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

    public void CompleteState()
    {
        if (StatusController.Instance == null) return;

        StatusController.Instance.IncreaseWorldTier();

        player.Attack = player.Attack * StatusController.Instance.CurrentStats.playerDamageNerf;
        player.SetStatPet();
    }
}
