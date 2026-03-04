using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class CornerSpeedControl : MonoBehaviour
{
    public float maxSpeed = 10f;
    public float minSpeed = 1f;

    private NavMeshAgent agent;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = maxSpeed;
    }

    void Update()
    {
        if (agent.path == null || agent.path.corners.Length < 3)
        {
            agent.speed = maxSpeed;
            return;
        }

        Vector3 a = agent.path.corners[0];
        Vector3 b = agent.path.corners[1];
        Vector3 c = agent.path.corners[2];

        Vector3 ab = (b - a).normalized;
        Vector3 bc = (c - b).normalized;

        float angle = Vector3.Angle(ab, bc);

        // Plus l’angle est grand, plus on ralentit
        float t = Mathf.InverseLerp(0f, 120f, angle);
        agent.speed = Mathf.Lerp(maxSpeed, minSpeed, t);
    }
}
