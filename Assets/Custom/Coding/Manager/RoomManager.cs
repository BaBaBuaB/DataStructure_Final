using UnityEngine;

public class RoomManager : MonoBehaviour
{
    [SerializeField]private GameObject[] gates;
    [SerializeField] private int enemiesCount;

    public GameObject[] spawnPoints;

    private int GetEnemiesCount()
    {
        return enemiesCount;
    }

    private void RoomComplete()
    {
        
    }
}
