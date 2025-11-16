using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    public GameObject WinScreen;
    public GameObject LoseScreen;

    public GameObject PopUp;

    void Start()
    {
        WinScreen.SetActive(false);
        LoseScreen.SetActive(false);

        PopUp.SetActive(false);
    }

    void Update()
    {
        //hold tab to see all slime's skill
        if (Input.GetKey(KeyCode.Tab))
        {
            PopUp.SetActive(true);
        }

        else if (Input.GetKeyUp(KeyCode.Tab))
        {
            PopUp.SetActive(false);
        }
    }

    public void StartButton()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void ExitButton()
    {
        Application.Quit();
    }

    public void RetryButton()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void WinScene()
    {
        WinScreen.SetActive(true);
    }

    public void LoseScene()
    {
        LoseScreen.SetActive(true);
    }
}
