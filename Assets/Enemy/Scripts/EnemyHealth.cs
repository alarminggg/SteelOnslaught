using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int health = 100;

    public HealthBar healthBar;
    public Spawner spawner;


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
        Debug.Log("Enemy has died");
        spawner.enemiesKilled++;
        Destroy(gameObject);
        spawner.totalKills++;
    }
}