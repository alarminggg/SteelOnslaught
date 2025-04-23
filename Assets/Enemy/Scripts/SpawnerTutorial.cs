using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SpawnerTutorial : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform[] spawnPoints = new Transform[3];

    private GameObject[] currentEnemies = new GameObject[3];

    private void Start()
    {
        
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            SpawnEnemyAt(i);
        }
    }

    public void SpawnEnemyAt(int index)
    {
        if (spawnPoints[index] == null)
        {
            Debug.LogError("Missing spawn point at index: " + index);
            return;
        }

        GameObject newEnemy = Instantiate(enemyPrefab, spawnPoints[index].position, Quaternion.identity);

        EnemyHealthTutorial enemyHealthTutrotial = newEnemy.GetComponent<EnemyHealthTutorial>();
        if (enemyHealthTutrotial != null)
        {
            enemyHealthTutrotial.spawnerTutorial = this;
            enemyHealthTutrotial.spawnIndex = index;
        }

        currentEnemies[index] = newEnemy;
    }
}

