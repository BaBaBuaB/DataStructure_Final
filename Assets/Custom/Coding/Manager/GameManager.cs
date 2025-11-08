using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    [SerializeField]private UIManager UIManager;
    public int worldTier;

    public static GameManager GetInstance()
    {
        return instance;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CheckCompleteState()
    {
        
    }
}
