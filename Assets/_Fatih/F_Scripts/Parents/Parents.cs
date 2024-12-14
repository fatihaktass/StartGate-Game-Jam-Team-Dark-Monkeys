using UnityEngine;
using UnityEngine.AI;

public class Parents : MonoBehaviour
{
    [SerializeField] bool detectingPlayer = false;
    [SerializeField] bool goToWalkPoint = false;
    private Vector3 walkPoint;
    [SerializeField] Transform playerTransform;

    NavMeshAgent agent;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        detectingPlayer = Physics.CheckSphere(transform.position, 10f, LayerMask.GetMask("Player"));

        if (detectingPlayer && IsPlayerVisible(playerTransform))
        {
            goToWalkPoint = false;
            agent.SetDestination(playerTransform.position);
        }
        else
        {
            Patrolling();
        }
    }

    private void Patrolling()
    {
        if (!goToWalkPoint)
        {
            float randomX = Random.Range(-50f, 50f);
            float randomZ = Random.Range(-50f, 50f);

            walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

            if (NavMesh.SamplePosition(walkPoint, out NavMeshHit hit, 2f, NavMesh.AllAreas))
            {
                walkPoint = hit.position;
                goToWalkPoint = true;
            }
        }
        else
        {
            agent.SetDestination(walkPoint);
        }

        if (Vector3.Distance(transform.position, walkPoint) < 2f + agent.stoppingDistance)
        {
            goToWalkPoint = false;
        } 
    }

    private bool IsPlayerVisible(Transform player)
    {
        Vector3 directionToPlayer = player.position - transform.position;

        if (Physics.Raycast(transform.position, directionToPlayer.normalized, out RaycastHit hit, directionToPlayer.magnitude))
        {
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Default"))
            {
                return false;
            }
        }

        return true;
    }
}
