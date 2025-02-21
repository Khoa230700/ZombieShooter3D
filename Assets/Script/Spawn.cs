using System.Collections;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Transform[] spawnPoints = new Transform[3];
    private int Enemy = 0;
    private void Start()
    {
        
        // Khởi chạy coroutine để spawn quái
        StartCoroutine(SpawnEnemies());
    }
    IEnumerator SpawnEnemies()
    {
       
        while (Enemy <= 60)
        {
            for (int i = 0; i < spawnPoints.Length; i++)
            {
                Instantiate(enemyPrefab, spawnPoints[i].position , spawnPoints[i].rotation);
                Enemy++;
                Debug.Log(Enemy);
                yield return new WaitForSeconds(2);
            }
            yield return new WaitForSeconds(5);
        }
    }

}

