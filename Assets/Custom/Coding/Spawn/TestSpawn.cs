using System.Collections;
using UnityEngine;

public class TestSpawn : MonoBehaviour
{

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Invoke("test", 3f);
    }
    
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            GameObject c = BaseSpawn.instance.Spawn("Test");
            Test(c);
        }
    }

    void Test(GameObject c)
    {
        StartCoroutine(wait());
        IEnumerator wait()
        {
            yield return new WaitForSeconds(3);
            BaseSpawn.instance.Return(c, "Test");
        }
    }

    // Update is called once per frame
}
