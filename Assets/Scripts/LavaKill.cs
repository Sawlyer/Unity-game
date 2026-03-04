using UnityEngine;

public class LavaKill : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            GameManager.I.Lose();
    }
}
