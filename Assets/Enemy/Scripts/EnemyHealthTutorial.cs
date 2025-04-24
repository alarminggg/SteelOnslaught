using UnityEngine;

public class EnemyHealthTutorial : MonoBehaviour
{
    public int health = 100;

    public HealthBar healthBar;
    public SpawnerTutorial spawnerTutorial;
    public int spawnIndex;

    private void Start()
    {

        if (healthBar != null)
        {
            healthBar.SetMaxHealth(health);
        }
    }
    public void TakeDamage(int damage)
    {
        health -= damage;

        healthBar.setHealth(health);

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
