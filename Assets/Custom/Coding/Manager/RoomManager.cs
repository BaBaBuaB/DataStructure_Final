using UnityEngine;
using UnityEngine.Rendering;

public class RoomManager : MonoBehaviour
{
    [SerializeField]private GameObject[] gates;
    [SerializeField]private EnemySpawner[] spawners;
    [SerializeField] private GameObject prize;

    private int enemiesInScene; //จำนวน enemy ใน scene ปัจจุบัน
    [SerializeField] private int enemiesCount; //จำนวนที่เหลือสำหรับ spawn เพิ่ม

    private Player player;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;
        player = collision.GetComponent<Player>();

        enemiesCount += StatusController.Instance.CurrentStats.extraMonsterCap;

        // ล็อคห้อง //
        foreach (var gate in gates)
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
        Debug.Log("Complete Room!");
        var prizes = ObjectPool.instance.Spawn("Key");
        prizes.transform.SetLocalPositionAndRotation(prize.transform.position,prize.transform.rotation);
        player.SummonPets();

        foreach(var gate in gates)
        {
            gate.SetActive(false);
        }

        if (gameObject.name == "Room_Boss")
        {
            GameManager.GetInstance().CompleteState();

            if (StatusController.Instance.CurrentWorldTier > 3)
            {
                UIManager.instance.CallGameWonUi();
            }
        }

        Destroy(gameObject);
    }
}
