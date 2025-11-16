using System;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private string enemyTag;
   public void Spawn()
    {
        GameObject enemy = ObjectPool.instance.Spawn(enemyTag);
        //เชื่อมกับ enemy เเล้วยัดค่า roomanager ปัจจบันให้
    }
}
