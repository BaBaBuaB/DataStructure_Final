using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    #region Singleton
    public static ObjectPool instance;
    #endregion


    
    [System.Serializable]
    public class Pool
    {
        //คลาสพิเศษเพื่อเก็บ tag ของ object ให้เพิ่มหรือปรับเอาในหน้า inspector
        public string tag;
        public GameObject prefab;
    }

    public List<Pool> pools; //เก็บค่าของ pool ทั้งหมด
    public Dictionary<string, Stack<GameObject>> poolDict; //เก็บ tag ของ pool นั้นกับจำนวน object ที่ถูกสร้างขึ้นมาเเล้ว

    void Awake()
    {
        // set singleton
        instance = this;

        poolDict = new Dictionary<string, Stack<GameObject>>();
        foreach(Pool pool in pools) //ยัดค่า pool ทั้งหมดเข้า dictionary
        {
            poolDict.Add(pool.tag, new Stack<GameObject>());
            CreateNewsObjects(pool.tag);
        }
    }

    private void CreateNewsObjects(string tag)
    {
        foreach (Pool pool in pools)
        {
            if (pool.tag == tag) //loop เช็คว่า pool ไหนมี tag เหมือนกัน
            {
                //เพิ่มเข้า pool
                GameObject c = Instantiate(pool.prefab);
                poolDict[pool.tag].Push(c);
                c.SetActive(false);
            }
        }
    }


    public void Return(GameObject obj, string tag) //tag ต้องเขียนให้เหมือนกับที่สร้าง
    {
        if (poolDict.ContainsKey(tag))
        {
            poolDict[tag].Push(obj);
            obj.SetActive(false);
        }
        else
        {
            Debug.Log("tag doesn't exist");
        }
    }
     
    public virtual GameObject Spawn(string tag)
    {
        if(!poolDict.ContainsKey(tag))
        {
            //กรณีใส่ tag ผิด
            Debug.Log("tag doesn't exist");
            return null;
        }

        //ถ้า pool หมดให้สร้างเพิ่ม
        if (poolDict[tag].Count == 0)
        {
            CreateNewsObjects(tag);
        }

        GameObject obj = poolDict[tag].Pop();
        obj.SetActive(true);

        return obj;
     }


}
