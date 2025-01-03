using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Parents : MonoBehaviour
{
    [SerializeField] bool detectingPlayer = false;
    [SerializeField] bool goToWalkPoint = false;
    private Vector3 walkPoint;
    [SerializeField] Transform playerTransform;

    NavMeshAgent agent;
    Animator anim;
    GameManager _gameManager;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        _gameManager = FindAnyObjectByType<GameManager>();

        StartCoroutine(SendRay());
    }

    private void Update()
    {
        detectingPlayer = Physics.CheckSphere(transform.position, 10f, LayerMask.GetMask("Player"));

        if (detectingPlayer && IsPlayerVisibleAndFacing(playerTransform))
        {
            goToWalkPoint = false;
            agent.SetDestination(playerTransform.position);
            anim.SetBool("isRunning", true);


            if (Vector3.Distance(transform.position, playerTransform.position) < 2.5f)
            {
                transform.LookAt(new Vector3(playerTransform.position.x,
                                                transform.position.y,
                                                   playerTransform.position.z));
                Debug.Log("oyuncu yakaland�");
                _gameManager.FinishTheGame(false);
            }
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
            anim.SetBool("isRunning", false);
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

    private bool IsPlayerVisibleAndFacing(Transform player)
    {
        Vector3 directionToPlayer = player.position - transform.position;

        if (Physics.Raycast(transform.position, directionToPlayer.normalized, out RaycastHit hit, directionToPlayer.magnitude))
        {
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Blockers"))
            {
                return false;
            }
        }

        float angle = Vector3.Angle(transform.forward, directionToPlayer);

        if (angle < 150f || Vector3.Distance(transform.position, player.position) < 4f)
        {
            return true;
        }

        return false;
    }


    IEnumerator SendRay()
    {
        while (true)
        {
            Ray ray = new Ray(transform.position + Vector3.up * 1f, transform.forward);

            if (Physics.Raycast(ray, out RaycastHit hit, 5f))
            {
                if (hit.collider.CompareTag("Door"))
                {
                    Debug.Log("NPC kap�y� buldu!");

                    Doors doorScript = hit.collider.GetComponent<Doors>();
                    if (doorScript != null)
                    {
                        doorScript.DoorMovements(); 
                    }
                }
            }

            yield return new WaitForSeconds(0.5f);
        }
        
    }
}
