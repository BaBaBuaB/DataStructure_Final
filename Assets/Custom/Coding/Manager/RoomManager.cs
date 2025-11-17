using UnityEngine;
using UnityEngine.Rendering;

public class RoomManager : MonoBehaviour
{
    [SerializeField]private GameObject[] gates;
    [SerializeField]private EnemySpawner[] spawners;
    

    private int enemiesInScene; //จำนวน enemy ใน scene ปัจจุบัน
    [SerializeField] private int enemiesCount; //จำนวนที่เหลือสำหรับ spawn เพิ่ม


    private void OnTriggerEnter2D(Collider2D collision)
    {
        // ล็อคห้อง //
        foreach(var gate in gates)
        {
            gate.SetActive(true);
        }

        // spawn monster //
        foreach(var spawner in spawners)
        {
            spawner.Spawn();
            enemiesCount--;
            enemiesInScene++;
        }
    }

    //เรียกหลัง enemies ตาย//
    public void OnEnemiesDeath()
    {
        enemiesInScene--;

        //Spawn monster เพิ่มหาก enemies count ยังเหลือ//
        if(enemiesCount > 0)
        {
            int r = Random.Range(0,spawners.Length);
            spawners[r].Spawn();
            enemiesCount--;
            enemiesInScene++;
        }

        //เช็คว่า monster ถูกฆ่าทั้งหมดหรือยัง//
        if(enemiesInScene <= 0)
        {
            RoomComplete();
        }
    }
    private void RoomComplete()
    {
        foreach(var gate in gates)
        {
            gate.SetActive(false);
        }
    }
}
