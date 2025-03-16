using System.Collections;
using UnityEngine;
using TMPro;

public class Spawn : MonoBehaviour
{
    [SerializeField] private GameObject[] enemyPrefabs;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private int waveNumber = 1;
    [SerializeField] private int zombiesPerWave = 5;
    [SerializeField] private float spawnDelay = 2f;
    [SerializeField] private float waveDelay = 5f;
    [SerializeField] private int maxWave = 3;
    public int zombiesRemaining;

    [SerializeField] private TextMeshProUGUI textMeshProWave;
    [SerializeField] private TextMeshProUGUI textMeshProZom;
    [SerializeField] private TextMeshProUGUI textMeshCooldown;
    [SerializeField] private GameObject panelcooldownl;

    private void Start()
    {
        StartCoroutine(StartNextWave());
    }

    private void Update()
    {
        textMeshProWave.text = waveNumber.ToString();
        textMeshProZom.text = zombiesRemaining.ToString();
    }

    IEnumerator StartNextWave()
    {
        float currentDelay = waveDelay;
        while (currentDelay > 0)
        {
            panelcooldownl.SetActive(true);
            textMeshCooldown.text = currentDelay.ToString("F0");
            currentDelay -= Time.deltaTime;
            yield return null;
        }

        panelcooldownl.SetActive(false);
        textMeshCooldown.text = "0";

        if (waveNumber <= maxWave)
        {
            StartCoroutine(SpawnWave());
        }
        else
        {
            Debug.Log("Tất cả các wave đã được hoàn thành!");
        }
    }

    public void ZombieKilled()
    {
        if (zombiesRemaining <= 0)
        {
            waveNumber++;
            StartCoroutine(StartNextWave());
        }
    }

    IEnumerator SpawnWave()
    {
        zombiesRemaining = waveNumber * zombiesPerWave;
        int rounds = zombiesRemaining / spawnPoints.Length;
        int remainder = zombiesRemaining % spawnPoints.Length;

        for (int i = 0; i < rounds; i++)
        {
            for (int j = 0; j < spawnPoints.Length; j++)
            {
                SpawnRandomZombie(spawnPoints[j]);
                yield return new WaitForSeconds(spawnDelay);
            }
        }

        for (int j = 0; j < remainder; j++)
        {
            SpawnRandomZombie(spawnPoints[j]);
            yield return new WaitForSeconds(spawnDelay);
        }
    }

    private void SpawnRandomZombie(Transform spawnPoint)
    {
        if (enemyPrefabs.Length < 2)
        {
            Debug.LogError("Thiếu prefab zombie! Cần ít nhất 2 prefab.");
            return;
        }

        float rand = Random.value;
        GameObject selectedPrefab = (rand <= 0.7f) ? enemyPrefabs[0] : enemyPrefabs[1];

        Instantiate(selectedPrefab, spawnPoint.position, spawnPoint.rotation);
    }
}
