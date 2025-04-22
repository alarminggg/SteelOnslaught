using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class OutOfBoundsDeath : MonoBehaviour
{
    [SerializeField] public float damage = 1000f;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }
        }
    }
}




