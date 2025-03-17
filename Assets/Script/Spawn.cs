using System.Collections;
using UnityEngine;
using TMPro;

public class Spawn : MonoBehaviour
{
    [Header("Danh sách prefab zombie & danh sách điểm spawn zombies")]
    [SerializeField] private GameObject[] enemyPrefabs;
    [SerializeField] private Transform[] spawnPoints;

    [Header("Thời gian")]
    [SerializeField] private float spawnInterval = 3f;
    [SerializeField] private float waveInterval = 40f;
    public bool isWaveTime = false;
    public bool isWaveActive = false;

    [Header("Hệ thống điều chỉnh wave")]
    private int waveNumber = 0;
    private float healthMultiplier = 1f;
    private float speedMultiplier = 1f;
    private float damageMultiplier = 1f;
    private int zombiesRemaining = 0;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI textMeshProWave;
    [SerializeField] private TextMeshProUGUI textMeshProCountdown;

    [Header("Audio")]
    [SerializeField] private AudioSource warningSound;

    private Coroutine blinkCoroutine;

    private void Start()
    {
        StartCoroutine(SpawnZombiesRoutine());
        StartCoroutine(WaveManager());
    }

    private IEnumerator SpawnZombiesRoutine()
    {
        while (true)
        {
            foreach (Transform spawnPoint in spawnPoints)
            {
                SpawnRandomZombie(spawnPoint);
            }
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private IEnumerator WaveManager()
    {
        while (true)
        {
            yield return StartCoroutine(StartWaveCountdown());
            StartCoroutine(HandleWave());
        }
    }

    private IEnumerator StartWaveCountdown()
    {
        float remainingTime = waveInterval;
        textMeshProCountdown.color = Color.white;
        textMeshProCountdown.text = $"{remainingTime:F0}s";

        while (remainingTime > 0)
        {
            textMeshProCountdown.text = $"{remainingTime:F0}s";

            if (remainingTime <= 5)
            {
                textMeshProCountdown.color = Color.red;

                if (remainingTime == 5)
                {
                    if (warningSound != null && !warningSound.isPlaying)
                    {
                        warningSound.Play();
                    }
                }
            }

            if (remainingTime <= 3)
            {
                if (blinkCoroutine == null)
                {
                    blinkCoroutine = StartCoroutine(BlinkCountdownText(0.5f));
                }
            }

            if (remainingTime == 1)
            {
                if (blinkCoroutine != null)
                {
                    StopCoroutine(blinkCoroutine);
                }
                blinkCoroutine = StartCoroutine(BlinkCountdownText(0.2f));
            }

            yield return new WaitForSeconds(1f);
            remainingTime--;
        }

        textMeshProCountdown.text = "";
        if (blinkCoroutine != null)
        {
            StopCoroutine(blinkCoroutine);
            blinkCoroutine = null;
        }
    }

    private IEnumerator BlinkCountdownText(float interval)
    {
        while (true)
        {
            textMeshProCountdown.enabled = !textMeshProCountdown.enabled;
            yield return new WaitForSeconds(interval);
        }
    }

    private IEnumerator HandleWave()
    {
        if (warningSound != null)
        {
            warningSound.Stop();
        }

        isWaveActive = true;
        waveNumber++;
        textMeshProWave.text = $"{waveNumber}";
        ApplyWaveBuff();

        foreach (Transform spawnPoint in spawnPoints)
        {
            for (int i = 0; i < 2; i++)
            {
                SpawnRandomZombie(spawnPoint);
                yield return new WaitForSeconds(0.5f);
            }
        }

        yield return new WaitForSeconds(2f);
        isWaveActive = false;
        isWaveTime = false;
    }

    private void ApplyWaveBuff()
    {
        switch (waveNumber % 3)
        {
            case 1:
                healthMultiplier *= 1.1f;
                break;
            case 2:
                speedMultiplier *= 1.02f;
                break;
            case 0:
                damageMultiplier *= 1.08f;
                break;
        }

        Debug.Log($"[Wave {waveNumber}] - Health x{healthMultiplier}, Speed x{speedMultiplier}, Damage x{damageMultiplier}");
    }

    private void SpawnRandomZombie(Transform spawnPoint)
    {
        if (enemyPrefabs.Length < 2)
        {
            Debug.LogError("⚠️ Thiếu prefab zombie! Cần ít nhất 2 prefab.");
            return;
        }

        float rand = Random.value;
        GameObject selectedPrefab = (rand <= 0.7f) ? enemyPrefabs[0] : enemyPrefabs[1];

        GameObject zombieInstance = Instantiate(selectedPrefab, spawnPoint.position, spawnPoint.rotation);
        Zombie zombieScript = zombieInstance.GetComponent<Zombie>();

        if (zombieScript != null)
        {
            int baseHealth = 8;
            float baseSpeed = 3f;
            int baseDamage = 2;

            int newHealth = Mathf.RoundToInt(baseHealth * healthMultiplier);
            float newSpeed = baseSpeed * speedMultiplier;
            int newDamage = Mathf.RoundToInt(baseDamage * damageMultiplier);

            zombieScript.Initialize(newDamage, newHealth, newSpeed);
            Debug.Log($"🧟 Zombie Spawned: Health={newHealth}, Speed={newSpeed}, Damage={newDamage}");
        }
    }

    public void ZombieKilled()
    {
        //zombiesRemaining--;
        Debug.Log($"Zombie killed! Remaining: {zombiesRemaining}");

        //if (zombiesRemaining <= 0)
        //{
        //    Debug.Log("Wave Completed!");
        //    isWaveTime = true;
        //}
    }
}
