using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] powerUpPrefabs;
    public GameObject[] enemyPrefabs;

    public GameObject bossPrefab;
    public GameObject[] miniEnemyPrefabs;
    public int bossRound;

    private int randomEnemy;
    private int randomPowerUp;
    private float spawnRange = 9;
    public int enemyCount;
    public int waveNumber = 1;

    private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        randomPowerUp = Random.Range(0, powerUpPrefabs.Length);
        Instantiate(powerUpPrefabs[randomPowerUp], GenerateSpawnPosition(), powerUpPrefabs[randomPowerUp].transform.rotation);
        SpawnEnemyWave(waveNumber);
        gameManager = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.isGameActive == true)
        {
            enemyCount = FindObjectsOfType<Enemy>().Length;

            if (enemyCount == 0)
            {
                waveNumber++;
                if (waveNumber % bossRound == 0)
                    SpawnBossWave(waveNumber);
                else
                    SpawnEnemyWave(waveNumber);
                randomPowerUp = Random.Range(0, powerUpPrefabs.Length);
                Instantiate(powerUpPrefabs[randomPowerUp], GenerateSpawnPosition(), powerUpPrefabs[randomPowerUp].transform.rotation);
                gameManager.UpdateRound();
            }
        }
    }

    void SpawnEnemyWave(int enemiesToSpawn)
    {
        for (int i = 0; i < enemiesToSpawn; i++)
        {
            randomEnemy = Random.Range(0, enemyPrefabs.Length);
            Instantiate(enemyPrefabs[randomEnemy], GenerateSpawnPosition(), enemyPrefabs[randomEnemy].transform.rotation);
        }
    }

    private Vector3 GenerateSpawnPosition()
    {
        float spawnPosZ = Random.Range(-spawnRange, spawnRange);
        float spawnPosX = Random.Range(-spawnRange, spawnRange);

        Vector3 randomPos = new Vector3(spawnPosX, 0, spawnPosZ);

        return randomPos;
    }

    void SpawnBossWave(int currentRound)
    {
        int miniEnemysToSpawn;
        if (bossRound != 0)
            miniEnemysToSpawn = currentRound / bossRound;
        else
            miniEnemysToSpawn = 1;
        var boss = Instantiate(bossPrefab, GenerateSpawnPosition(), bossPrefab.transform.rotation);
        boss.GetComponent<Enemy>().miniEnemySpawnCount = miniEnemysToSpawn;
    }

    public void SpawnMiniEnemy(int amount)
    {
        for(int i = 0; i < amount; i++)
        {
            int randomMini = Random.Range(0, miniEnemyPrefabs.Length);
            Instantiate(miniEnemyPrefabs[randomMini], GenerateSpawnPosition(), miniEnemyPrefabs[randomMini].transform.rotation);
        }
    }
}
