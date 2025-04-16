using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    private NavMeshAgent agent = null;
    [SerializeField] public Transform target;
    [SerializeField] public float attackRange = 2f;
    [SerializeField] public float damage = 20f;
    [SerializeField] public float attackCooldown = 1.5f;
    [SerializeField] private float attackWindupDelay = 1f;

    private bool isWindingUp = false;
    private float windUpStartTime = 0f;

    private float lastAttackTime = 0f;

    private void Start()
    {
        GetReference();
    }

    private void Update()
    {
        MoveToTarget();
        LookAtTarget();
        TryAttackPlayer();
    }

    private void MoveToTarget()
    {
        if (target != null)
        {
            agent.SetDestination(target.position);
        }
    }

    private void LookAtTarget()
    {
        Vector3 targetPosition = target.position;
        targetPosition.y = transform.position.y;


        Vector3 direction = (targetPosition - transform.position).normalized;

        if (Mathf.Abs(target.position.y - transform.position.y) < 1f)
        {

            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        }
    }

    private void TryAttackPlayer()
    {
        if (target == null)
        return;

        float distance = Vector3.Distance(transform.position, target.position);

        if (distance <= attackRange)
        {
            if (!isWindingUp && Time.time > lastAttackTime + attackCooldown)
            {
                isWindingUp = true;
                windUpStartTime = Time.time;
            }

            if (isWindingUp && Time.time >= windUpStartTime + attackWindupDelay)
            {
                PlayerHealth playerHealth = target.GetComponent<PlayerHealth>();
                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(damage);
                    Debug.Log("Enemy Hit the Player!");
                }

                lastAttackTime = Time.time;
                isWindingUp = false;
            }
        }
        else
        {
            isWindingUp = false;
        }
    }

    private void GetReference()
    {
        agent = GetComponent<NavMeshAgent>();

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
