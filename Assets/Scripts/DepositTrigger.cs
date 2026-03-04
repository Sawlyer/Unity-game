using UnityEngine;

public class DepositTrigger : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        GameManager.I.TryDeposit();
    }
}
