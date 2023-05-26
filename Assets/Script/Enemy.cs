using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float speed;
    private Rigidbody enemyRb;
    private GameObject player;

    [SerializeField] private bool isBoss = false;
    [SerializeField] private float spawnInterval;
    public int miniEnemySpawnCount;
    private float nextSpawn;

    private SpawnManager spawnManager;
    private GameManager gameManager;

    [SerializeField] private int scoreValue;

    // Start is called before the first frame update
    void Start()
    {
        enemyRb= GetComponent<Rigidbody>();
        player = GameObject.Find("Player");
        gameManager = FindObjectOfType<GameManager>();

        if (isBoss)
            spawnManager = FindObjectOfType<SpawnManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.isGameActive)
        {
            Vector3 lookDirection = (player.transform.position - transform.position).normalized;
            enemyRb.AddForce(lookDirection * speed);

            if (isBoss)
            {
                spawnInterval = -0.25f * spawnManager.waveNumber + 5;
                if (spawnInterval < 1)
                    spawnInterval = 1;
                if (Time.time > nextSpawn)
                {
                    nextSpawn = Time.time + spawnInterval;
                    spawnManager.SpawnMiniEnemy(miniEnemySpawnCount);
                }
            }
        }
        if (transform.position.y < -10)
        {
            Destroy(gameObject);
            if (gameManager.isGameActive)
            {
                gameManager.score += scoreValue;
                gameManager.UpdateScore();
            }
        }
    }
}
