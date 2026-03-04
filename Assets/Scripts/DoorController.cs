using UnityEngine;
using UnityEngine.AI;

public class DoorController : MonoBehaviour
{
    public NavMeshObstacle obstacle;     // sur la porte
    public float openAngle = 90f;        // mets -90 si ça ouvre du mauvais côté
    public float speed = 180f;           // degrés/sec

    public bool isOpen = false;

    Quaternion closedRot;
    Quaternion openRot;

    void Awake()
    {
        closedRot = transform.localRotation;
        openRot = closedRot * Quaternion.Euler(0f, openAngle, 0f);

        if (obstacle == null)
            obstacle = GetComponent<NavMeshObstacle>();

        // par défaut porte fermée -> obstacle actif
        if (obstacle != null) obstacle.enabled = true;
    }

    void Update()
    {
        Quaternion target = isOpen ? openRot : closedRot;
        transform.localRotation = Quaternion.RotateTowards(transform.localRotation, target, speed * Time.deltaTime);

        // obstacle actif seulement quand fermé (et bien fermé)
        if (obstacle != null)
        {
            if (isOpen) obstacle.enabled = false;
            else
            {
                if (Quaternion.Angle(transform.localRotation, closedRot) < 1f)
                    obstacle.enabled = true;
            }
        }
    }

    public void Toggle()
    {
        isOpen = !isOpen;
    }
}
