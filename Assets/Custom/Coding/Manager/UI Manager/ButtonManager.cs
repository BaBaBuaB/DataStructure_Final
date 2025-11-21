using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
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

}
