using UnityEngine;

public class BaseSpawn : MonoBehaviour
{
    public GameObject[] prefabs;
    public int pools;

    public void CreateNewsObjects()
    {
        
    }

    public void Return()
    {
        
    }

    public virtual GameObject Spawn() 
    {
        return null;
    }
}
