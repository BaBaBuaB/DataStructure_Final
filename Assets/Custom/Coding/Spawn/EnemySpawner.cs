using System;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
   [SerializeField] private string enemyTag;
   [SerializeField] private RoomManager roomManager;
   public void Spawn()
    {
        GameObject enemy = ObjectPool.instance.Spawn(enemyTag);
        //เชื่อมกับ enemy เเล้วยัดค่า roomanager ปัจจบันให้
        var e = enemy.gameObject.GetComponent<Enemies>();

        enemy.transform.SetLocalPositionAndRotation(gameObject.transform.position,gameObject.transform.rotation);
        e.roomManager = roomManager;
    }
}
