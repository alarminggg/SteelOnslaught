using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    private int waveNumber = 0;
    public int enemySpawnAmount = 0;
    public int enemiesKilled = 0;


    public GameObject[] spawners;
    public GameObject enemy;

    private void Start()
    {
        spawners = new GameObject[4];

        for (int i = 0; i < spawners.Length; i++)
        {
            spawners[i] = transform.GetChild(i).gameObject;
        }

        StartWave();
    }

    private void Update()
    {
        if(Input.GetKeyUp(KeyCode.T))
        {
            SpawnEnemy();
        }

        if(enemiesKilled >= enemySpawnAmount)
        {
            NextWave();
        }
    }

    private void SpawnEnemy()
    {
        int spawnerID = Random.Range(0, spawners.Length);
        GameObject newEnemy = Instantiate(enemy, spawners[spawnerID].transform.position, spawners[spawnerID].transform.rotation);

        EnemyHealth enemyHealth = newEnemy.GetComponent<EnemyHealth>();
        if (enemyHealth != null)
        {
            enemyHealth.spawner = this;
        }
    }

    private void StartWave()
    {
        waveNumber = 1;
        enemySpawnAmount = 2;
        enemiesKilled = 0;

        for (int i = 0;i < enemySpawnAmount;i++)
        {
            SpawnEnemy();
        }
    }

    public void NextWave()
    {
        waveNumber++;
        enemySpawnAmount += 2;
        enemiesKilled = 0;

        for (int i = 0; i < enemySpawnAmount; i++)
        {
            SpawnEnemy();
        }
    }
}

