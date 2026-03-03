using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    public float maxUseDistance = 3f;
    public Camera cam;

    void Start()
    {
        if (cam == null) cam = Camera.main;
        if (cam == null) Debug.LogError("PlayerInteract: Camera.main introuvable (tag MainCamera ?).");
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (cam == null) return;

            Ray ray = new Ray(cam.transform.position, cam.transform.forward);
            if (Physics.Raycast(ray, out RaycastHit hit, maxUseDistance))
            {
                Debug.Log("Hit: " + hit.collider.name);

                // Cherche DoorController sur l'objet touché OU ses parents/enfants
                DoorController door =
                    hit.collider.GetComponent<DoorController>() ??
                    hit.collider.GetComponentInParent<DoorController>() ??
                    hit.collider.GetComponentInChildren<DoorController>();

                if (door != null)
                {
                    Debug.Log("Door found -> toggle");
                    door.Toggle();
                }
                else
                {
                    Debug.Log("No DoorController found on hit object hierarchy.");
                }
            }
            else
            {
                Debug.Log("No hit");
            }
        }
    }
}
