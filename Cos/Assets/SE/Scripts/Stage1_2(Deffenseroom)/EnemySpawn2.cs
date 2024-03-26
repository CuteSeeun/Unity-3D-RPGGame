using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn2 : MonoBehaviour
{
    public GameObject enemyPrefab; // 스폰할 적 프리팹
    public Transform[] spawnPoints; // 적이 스폰될 위치 배열
    public float initialSpawnDelay = 2f; // 초기 스폰 지연 시간
    public float repeatSpawnDelay = 2f; // 반복 스폰 간격
    public int enemiesPerWave = 3; // 한 번에 스폰될 적의 수
    public int totalWaves = 2; // 총 스폰할 wave 횟수

    private int currentWave = 0; // 현재 wave

    void Start()
    {
        // 초기 스폰 지연 후에 반복적으로 스폰 코루틴 호출
        InvokeRepeating("SpawnEnemies", initialSpawnDelay, repeatSpawnDelay);
    }

    void SpawnEnemies()
    {
        // 총 스폰할 wave 횟수를 넘으면 스폰 중지
        if (currentWave >= totalWaves)
        {
            CancelInvoke("SpawnEnemies");
            return;
        }

        // 현재 wave에서 스폰할 적 수만큼 반복해서 스폰
        for (int i = 0; i < enemiesPerWave; i++)
        {
            // 스폰 지점을 랜덤으로 선택
            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
            // 적 스폰
            Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
        }

        // 현재 wave 증가
        currentWave++;

        // 30초 후에 다음 wave 스폰
        if (currentWave >= totalWaves)
        {
            // 스폰 중지
            CancelInvoke("SpawnEnemies");
            return;
        }
    }
}
