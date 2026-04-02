using UnityEngine;

public class PlayerSoundEmitter : MonoBehaviour
{
    public float soundCooldown = 1f;
    private float lastSoundTime;

    void Update()
    {
        // Exemple simple : sauter produit un bruit
        if (Input.GetKeyDown(KeyCode.Space))
        {
            EmitSound();
        }
    }

    public void EmitSound()
    {
        if (Time.time - lastSoundTime < soundCooldown)
            return;

        lastSoundTime = Time.time;

        EnemyHearing[] enemies = FindObjectsOfType<EnemyHearing>();

        foreach (EnemyHearing enemy in enemies)
        {
            enemy.HearSound(transform.position);
        }

        Debug.Log("Player emitted sound at " + transform.position);
    }
}