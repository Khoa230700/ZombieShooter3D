using UnityEngine;
using System.Collections;

public class SpawnBullet : MonoBehaviour
{
    [Header("Điểm Spawn")]
    public Transform spawnPointA;
    public Transform spawnPointB;

    [Header("Prefab Bullet")]
    public GameObject prefabA;
    public GameObject prefabB;

    [Header("Thời gian spawn (giây)")]
    public float spawnInterval = 60f;

    void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    IEnumerator SpawnRoutine()
    {
        while (true)
        {
            SpawnAtPoint(spawnPointA, prefabA);
            SpawnAtPoint(spawnPointB, prefabB);
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void SpawnAtPoint(Transform spawnPoint, GameObject prefab)
    {
        if (spawnPoint != null && prefab != null)
        {
            Instantiate(prefab, spawnPoint.position, spawnPoint.rotation);
        }
    }
}
