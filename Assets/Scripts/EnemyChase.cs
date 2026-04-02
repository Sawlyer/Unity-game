using UnityEngine;
using UnityEngine.AI;

public class EnemyChase : MonoBehaviour
{
    public Transform target;

    private NavMeshAgent agent;
    private EnemyVision vision;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        vision = GetComponent<EnemyVision>();
    }

    void Update()
    {
        if (target == null) return;
        if (!agent.isOnNavMesh) return;

        if (vision != null && vision.canSeePlayer)
        {
            agent.SetDestination(target.position);
        }
    }
}