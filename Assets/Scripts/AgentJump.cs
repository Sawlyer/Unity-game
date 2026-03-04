using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class AgentJump : MonoBehaviour
{
    public float jumpHeight = 1.5f;
    public float jumpDuration = 1f;

    private NavMeshAgent agent;
    private bool isJumping;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.autoTraverseOffMeshLink = false; // on gère nous-mêmes
    }

    void Update()
    {
        if (!isJumping && agent.isOnOffMeshLink)
            StartCoroutine(Jump());
    }

    IEnumerator Jump()
    {
        isJumping = true;

        OffMeshLinkData data = agent.currentOffMeshLinkData;
        Vector3 startPos = transform.position;
        Vector3 endPos = data.endPos + Vector3.up * agent.baseOffset;

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / jumpDuration;

            // trajectoire (lerp) + parabole
            Vector3 pos = Vector3.Lerp(startPos, endPos, t);
            float y = 4f * jumpHeight * t * (1f - t); // bosse
            pos.y += y;

            transform.position = pos;
            yield return null;
        }

        agent.CompleteOffMeshLink();
        isJumping = false;
    }
}
