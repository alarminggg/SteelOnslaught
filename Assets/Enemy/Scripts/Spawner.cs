using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Spawner : MonoBehaviour
{
    private int waveNumber = 0;
    public int enemySpawnAmount = 0;
    public int enemiesKilled = 0;
    public int totalKills;

    private bool isWaitingForNextWave = false;

    public TextMeshProUGUI waveCounter;
    public TextMeshProUGUI waveNotif;
    public TextMeshProUGUI killsText;

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

        if(killsText != null)
        {
            killsText.text = "Kills: " + totalKills;
        }

        if(enemiesKilled >= enemySpawnAmount && !isWaitingForNextWave)
        {
            StartCoroutine(NextWave());
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
        enemySpawnAmount = 10;

        UpdateWaveUI();

        StartCoroutine(ShowWaveNotificationBeforeSpawning());
    }

    private IEnumerator ShowWaveNotificationBeforeSpawning()
    {
        
        waveNotif.text = "WAVE " + waveNumber;
        waveNotif.gameObject.SetActive(true);
        yield return new WaitForSeconds(2f); 
        waveNotif.gameObject.SetActive(false); 

        
        for (int i = 0; i < enemySpawnAmount; i++)
        {
            SpawnEnemy();
        }
    }



    private IEnumerator NextWave()
    {
        float delay = waveNumber >= 20 ? 3f : 5f;

        isWaitingForNextWave = true;

        waveNumber++;

        if (waveNotif != null)
        {
            waveNotif.text = "WAVE " + waveNumber;
            waveNotif.gameObject.SetActive(true);
        }

        yield return new WaitForSeconds(delay);

        waveNotif.gameObject.SetActive(false);

        
        enemySpawnAmount += 2;
        enemiesKilled = 0;

        UpdateWaveUI();

        
        for (int i = 0; i < enemySpawnAmount; i++)
        {
            SpawnEnemy();
        }

        isWaitingForNextWave = false; 
    }

    private void UpdateWaveUI()
    {
        if (waveCounter  != null)
        {
            waveCounter.text = "Wave: " + waveNumber;
        }
    }
}

