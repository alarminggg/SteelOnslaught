using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    private NavMeshAgent agent = null;
    [SerializeField] private Transform target;

    private void Start()
    {
        GetReference();
    }

    private void Update()
    {
        MoveToTarget();
        LookAtTarget();
    }

    private void MoveToTarget()
    {
        agent.SetDestination(target.position);
        
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

    private void GetReference()
    {
        agent = GetComponent<NavMeshAgent>();
    }
}
