using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class AttackPlayer : MonoBehaviour
{
    [SerializeField] public Transform target;
    [SerializeField] public float attackRange = 2f;
    [SerializeField] public float damage = 20f;
    [SerializeField] public float attackCooldown = 1.5f;

    private float lastAttackTime = 0f;

    private void Start()
    {
        GetReference();
    }

    private void Update()
    {
        TryAttackPlayer();
    }

    private void TryAttackPlayer()
    {
        if (target == null)
            return;

        float distance = Vector3.Distance(transform.position, target.position);

        if (distance <= attackRange && Time.time > lastAttackTime + attackCooldown)
        {
            PlayerHealth playerHealth = target.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
                lastAttackTime = Time.time;
                Debug.Log("Enemy Hit the Player!");
            }
        }
    }

    private void GetReference()
    {
        if (target == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                target = playerObj.transform;
            }

        }
    }
}
