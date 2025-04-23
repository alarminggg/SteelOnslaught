using UnityEngine;

public class EnemyHealthTutorial : MonoBehaviour
{
    public int health = 100;

    public SpawnerTutorial spawnerTutorial;
    public int spawnIndex;

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Enemy at spawn point " + spawnIndex + " died.");

        if (spawnerTutorial != null)
        {
            spawnerTutorial.SpawnEnemyAt(spawnIndex);
        }

        Destroy(gameObject);
    }
}
