using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    public float healRate = 10f;
    public float healDelay = 10f;

    public float currentHealth;
    private float lastDamageTime;
    private bool isHealing = false;

    public HealthBar healthBar;

    private PauseMenu pauseMenu;

    
    private void Start()
    {
        currentHealth = maxHealth;
        lastDamageTime = Time.time;

        healthBar.SetMaxHealth(maxHealth);

        pauseMenu = FindAnyObjectByType<PauseMenu>();
    }

    private void Update()
    {
        if(Time.time >= lastDamageTime + healDelay && currentHealth < maxHealth)
        {
            isHealing = true;
        }
        else if(currentHealth >= maxHealth)
        {
            isHealing = false;
        }
        if(isHealing)
        {
            PassiveHealing();
        }
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount; 
        Debug.Log("Player took damage, Current Health: " +  currentHealth);

        lastDamageTime = Time.time;
        isHealing = false;

        healthBar.setHealth(currentHealth);

        if(currentHealth <= 0f)
        {
            OnDeath();
        }
    }

    private void PassiveHealing()
    {
        currentHealth += healRate * Time.deltaTime;
        currentHealth = Mathf.Min(currentHealth, maxHealth);

        healthBar.setHealth(currentHealth);

        Debug.Log("Player is healing: " + currentHealth);
    }

    private void OnDeath()
    {
        pauseMenu.TriggerDeath();
        Debug.Log("Player is Dead!");
    }
}
