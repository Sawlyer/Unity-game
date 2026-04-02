using UnityEngine;

public class EnemyVision : MonoBehaviour
{
    public Transform player;
    public Transform eyePoint;

    public float viewDistance = 10f;
    public float viewAngle = 120f;

    public bool canSeePlayer { get; private set; }

    void Update()
    {
        canSeePlayer = false;

        if (player == null || eyePoint == null)
            return;

        Vector3 dirToPlayer = player.position - eyePoint.position;
        float distanceToPlayer = dirToPlayer.magnitude;

        // Trop loin
        if (distanceToPlayer > viewDistance)
            return;

        // Hors champ de vision
        float angle = Vector3.Angle(transform.forward, dirToPlayer);
        if (angle > viewAngle * 0.5f)
            return;

        // Vérifie si quelque chose bloque la vue
        Ray ray = new Ray(eyePoint.position, dirToPlayer.normalized);
        Debug.DrawRay(eyePoint.position, dirToPlayer.normalized * viewDistance, Color.red);

        if (Physics.Raycast(ray, out RaycastHit hit, viewDistance))
        {
            if (hit.collider.CompareTag("Player"))
            {
                canSeePlayer = true;
            }
        }
    }
}