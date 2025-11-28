using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    #region"Parameter"
    // UI manager จะควบคุมการทำงาน เปิด / ปิด ของ UI ทุกตัวยกเว้น mainmenu //

    //[SerializeField]private GameObject startGameUi;
    public static UIManager instance;
    [SerializeField]private HealthSlider healthUi;
    [SerializeField]private GameObject inGameUi;
    [SerializeField]private GameObject mainMenu;
    [SerializeField]private GameObject slimeStatsUi;
    [SerializeField]private GameObject gameOverUi;
    [SerializeField]private GameObject gameWinUi;
    #region"Worldtier Ui"
    [SerializeField] private GameObject worldTierUi;
    [SerializeField] private TMP_Text displayWorld;
    #endregion
    #region"CoolDown"
    [SerializeField] private TMP_Text cooldownDisplay;
    #endregion
    #endregion

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        CloseSlimeStatesUi();
        CloseGameOverUi();
        CloseGameWonUi();
    }

    // Main Menu //
    public void CallMainMenu()
    {
        mainMenu.SetActive(true);
    }

    public void CloseMainMenu()
    {
        mainMenu?.SetActive(false);
    }

    // Slime Stats //
    public void CallSlimeStatesUi()
    {
        slimeStatsUi.SetActive(true);
    }

    public void CloseSlimeStatesUi()
    {
        slimeStatsUi.SetActive(false);
    }

    // Game Over //
    public void CallGameOverUi() 
    {
        Time.timeScale = 0f;
        gameOverUi.SetActive(true);
    }
    public void CloseGameOverUi()
    {
        Time.timeScale = 1f;
        gameOverUi.SetActive(false);
    }

    // Gane Won //
    public void CallGameWonUi() 
    {
        Time.timeScale = 0f;
        gameWinUi.SetActive(true);
    }
    public void CloseGameWonUi()
    {
        Time.timeScale = 1f;
        gameWinUi.SetActive(false);
    }

    #region "World method"

    public void CallWorldTierUi()
    {
        worldTierUi.SetActive(true);
    }

    public void CloseWorldTier()
    {
        worldTierUi.SetActive(false);
    }

    public void UpdateWorldtierText(string text)
    {
        displayWorld.text = text;
    }
    #endregion

    #region"Barriar UI"

    public void UpdateCooldown(int secound)
    {
        cooldownDisplay.text = secound.ToString();
    }

    #endregion

    #region"Update UI"
    // Health Bar //
    public void HealthBarActive(bool active)
    {
        inGameUi.SetActive(active);
    }
    
    // HP //
    public void UpdateHealth(float hpValue,float maxHp)
    {
        //เรียก 1 ครั้งใน method awake ของ player จากนั้นเรียกทุกครั้งที่เลือดเพิ่มหรือลด
        healthUi.UpdateHP(hpValue,maxHp);
    }

    //Load Scene
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    #endregion
    #region "Button Active"
    public void ExitGame()
    {
        Application.Quit();
    }

    public void RetryGame()
    {
        LoadScene(SceneManager.GetActiveScene().name);
        HealthBarActive(true);
        CloseMainMenu();
        CloseGameOverUi();
        CloseGameWonUi();
    }

    public void GoToNextScene()
    {
        LoadScene("Dungeon_map");
        HealthBarActive(true);
        CloseMainMenu();
        CloseGameOverUi();
        CloseGameWonUi();
    }

    public void ReturnMainMenu()
    {
        LoadScene("MainMenu");
        HealthBarActive(false);
        CallMainMenu();
        CloseGameOverUi();
        CloseGameWonUi();
    }
    #endregion
}
