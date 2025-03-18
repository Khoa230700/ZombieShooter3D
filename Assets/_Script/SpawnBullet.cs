using UnityEngine;
using System.Collections;

public class SpawnBullet : MonoBehaviour
{
    [Header("Điểm Spawn")]
    public Transform spawnPointA;

    [Header("Prefab Bullet")]
    public GameObject prefabA;

    [Header("Thời gian spawn (giây)")]
    public float spawnInterval = 60f;

    public GameObject SettingPanel;


    private void Awake()
    {
        SettingPanel.SetActive(true);
    }
    void Start()
    {
        StartCoroutine(SpawnRoutine());
    }
    

    IEnumerator SpawnRoutine()
    {
        while (true)
        {
            SpawnAtPoint(spawnPointA, prefabA);
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
