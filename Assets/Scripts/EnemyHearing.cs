using UnityEngine;

public class EnemyHearing : MonoBehaviour
{
    public float hearingRange = 8f;

    public bool heardSound { get; private set; }
    public Vector3 lastHeardPosition { get; private set; }

    public void HearSound(Vector3 soundPosition)
    {
        float distance = Vector3.Distance(transform.position, soundPosition);

        if (distance <= hearingRange)
        {
            heardSound = true;
            lastHeardPosition = soundPosition;
            Debug.Log(name + " heard a sound at " + soundPosition);
        }
    }

    public void ClearSound()
    {
        heardSound = false;
    }
}