using UnityEngine;

public class EnemyTouchKill : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        Debug.Log($"EnemyKillTouch: hit {other.name} tag={other.tag}");

        if (other.CompareTag("Player"))
        {
            Debug.Log("EnemyKillOnTouch: PLAYER DETECTED -> Lose()");
            GameManager.I.Lose();
        }
    }
}
