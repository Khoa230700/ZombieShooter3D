using System.Collections;
using UnityEngine;
using TMPro;

public class Spawn : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;    // Prefab của zombie
    [SerializeField] private Transform[] spawnPoints;  // Mảng điểm spawn
    [SerializeField] private int waveNumber = 1;       // Số wave hiện tại
    [SerializeField] private int   zombiesPerWave = 5; // Số zombie mỗi wave cơ bản
    [SerializeField] private float spawnDelay = 2f;    // Thời gian delay giữa các lần spawn
    [SerializeField] private float waveDelay = 5f;     // Thời gian delay giữa các wave
    [SerializeField] private int maxWave = 3;
    public int zombiesRemaining;
    [SerializeField] private TextMeshProUGUI textMeshProWave;
    [SerializeField] private TextMeshProUGUI textMeshProZom;
    [SerializeField] private TextMeshProUGUI textMeshCooldown; // Tham chiếu đến UI Text để hiển thị thời gian
    [SerializeField] private GameObject panelcooldownl;

    private void Start()
    {
       
        // Khởi chạy coroutine để spawn quái
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
        // Vòng lặp giảm thời gian từ waveDelay về 0
        while (currentDelay > 0)
        {
            panelcooldownl.SetActive(true);
            textMeshCooldown.text = currentDelay.ToString("F0");
            currentDelay -= Time.deltaTime; // Giảm dần theo thời gian thực
            yield return null; // Chờ đến frame tiếp theo
        }
        panelcooldownl.SetActive(false);
        textMeshCooldown.text = "0"; // Đảm bảo hiển thị 0 khi hết thời gian
        // Sau khi đếm ngược xong, kiểm tra và bắt đầu wave tiếp theo
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
        if (zombiesRemaining <= 0 )
        {
            waveNumber++;
            StartCoroutine(StartNextWave());
        }
    }
    IEnumerator SpawnWave()
    {
        // Tính tổng số zombie cần spawn cho wave hiện tại.
        zombiesRemaining = waveNumber * zombiesPerWave;
        // Tính số vòng lặp đầy đủ (mỗi vòng spawn enemy tại tất cả các spawn point)
        int rounds = zombiesRemaining / spawnPoints.Length;
        // Tính số enemy còn lại nếu tổng không chia hết cho số spawn point
        int remainder = zombiesRemaining % spawnPoints.Length;

        // Vòng lặp thứ nhất: spawn theo từng vòng đầy đủ qua tất cả các spawn point.
        for (int i = 0; i < rounds; i++)
        {
            for (int j = 0; j < spawnPoints.Length; j++)
            {
                Instantiate(enemyPrefab, spawnPoints[j].position, spawnPoints[j].rotation);
                yield return new WaitForSeconds(spawnDelay);
            }
        }

        // Vòng lặp thứ hai: spawn các enemy còn lại (nếu có).
        for (int j = 0; j < remainder; j++)
        {
            Instantiate(enemyPrefab, spawnPoints[j].position, spawnPoints[j].rotation);
            yield return new WaitForSeconds(spawnDelay);
        }
    }
   
}

