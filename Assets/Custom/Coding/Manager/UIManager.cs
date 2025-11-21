using UnityEngine;

public class UIManager : MonoBehaviour
{
    // UI manager จะควบคุมการทำงาน เปิด / ปิด ของ UI ทุกตัวยกเว้น mainmenu //

    //[SerializeField]private GameObject startGameUi;
    public static UIManager instance;
    [SerializeField]private HealthSlider healthUi;
    [SerializeField]private GameObject slimeStatsUi;
    [SerializeField]private GameObject gameOverUi;
    [SerializeField]private GameObject gameWinUi;

    void Awake()
    {
        instance = this;

        CloseSlimeStatesUi();
        CloseGameOverUi();
        CloseGameWonUi();
    }

    void Update()
    {
        //hold tab to see all slime's skill
        if (Input.GetKey(KeyCode.Tab))
        {
            CallSlimeStatesUi();
        }

        else if (Input.GetKeyUp(KeyCode.Tab))
        {
            CloseSlimeStatesUi();
        }
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
        gameOverUi.SetActive(true);
    }
    public void CloseGameOverUi()
    {
        gameOverUi.SetActive(false);
    }

    // Gane Won //
    public void CallGameWonUi() 
    {
        gameWinUi.SetActive(true);
    }
    public void CloseGameWonUi()
    {
        gameWinUi.SetActive(false);
    }

    // HP //
    public void UpdateHealth(float hpValue)
    {
        //เรียก 1 ครั้งใน method awake ของ player จากนั้นเรียกทุกครั้งที่เลือดเพิ่มหรือลด
        healthUi.UpdateHP(hpValue);
    }
}
