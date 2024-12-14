using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class TimelineCharacter : MonoBehaviour
{
    [SerializeField] GameObject target;
    [SerializeField] GameObject disableObject;
    [SerializeField] GameObject gameScene;
    [SerializeField] GameObject inGamePanel;
    
    bool startedTimeline;
    
    NavMeshAgent agent;
    Animator anim;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (startedTimeline && !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            gameScene.SetActive(true);
            inGamePanel.SetActive(true);
            disableObject.SetActive(false);
            startedTimeline = false;
        }
    }

    public void GoToTarget()
    {
        startedTimeline = true;
        anim.SetBool("IsMoving", true);
        agent.SetDestination(target.transform.position);
    }
}
