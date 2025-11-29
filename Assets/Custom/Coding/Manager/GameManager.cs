using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public struct WorldTierStats
    {
        public float enemyHealthBuff, enemyDamageBuff, playerDamageNerf;
        public int extraMonsterCap;
        public int worldTier;
    }

    private static GameManager instance;
    [SerializeField]private Player player;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // (ถ้า GameManager ต้องอยู่ตลอด)
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

        if (player == null)
        {
            GameObject playerObj = GameObject.Find("Player");
            player = playerObj.GetComponent<Player>();
        }

        StatusController.Instance.IncreaseWorldTier();
        if (StatusController.Instance.CurrentWorldTier <= 3)
        {
            StartCoroutine(DebugState());
        }

        player.Attack = player.Attack * StatusController.Instance.CurrentStats.playerDamageNerf;
        player.SetStatPet();
    }

    private IEnumerator DebugState()
    {
        UIManager.instance.CallWorldTierUi();
        UIManager.instance.UpdateWorldtierText("Complete State!");
        yield return new WaitForSeconds(2f);
        UIManager.instance.UpdateWorldtierText($"Current world tier {StatusController.Instance.CurrentWorldTier}");
        yield return new WaitForSeconds(2f);
        UIManager.instance.CloseWorldTier();
    }
}
