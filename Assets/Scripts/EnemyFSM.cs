using UnityEngine;
using UnityEngine.AI;

public class EnemyFSM : MonoBehaviour
{
    public enum State
    {
        Patrol,
        Investigate,
        Alarm,
        Chase
    }

    public State currentState;

    [Header("References")]
    public Transform player;
    public Transform[] patrolPoints;

    [Header("Patrol Settings")]
    public float patrolWaitTime = 1.5f;
    public float patrolPointReachDistance = 1.2f;

    [Header("Investigate Settings")]
    public float investigateReachDistance = 1.2f;

    [Header("Chase Settings")]
    public float loseSightDelay = 2f;

    [Header("Alarm Settings")]
    public float alertCooldown = 1f;

    private NavMeshAgent agent;
    private EnemyVision vision;
    private EnemyHearing hearing;

    private int patrolIndex = 0;
    private float patrolWaitTimer = 0f;
    private float lostSightTimer = 0f;
    private float lastAlertTime = -999f;

    private Vector3 investigateTarget;
    private Vector3 lastKnownPlayerPosition;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        vision = GetComponent<EnemyVision>();
        hearing = GetComponent<EnemyHearing>();

        ChangeState(State.Patrol);
    }

    void Update()
    {
        switch (currentState)
        {
            case State.Patrol:
                UpdatePatrol();
                break;

            case State.Investigate:
                UpdateInvestigate();
                break;

            case State.Alarm:
                UpdateAlarm();
                break;

            case State.Chase:
                UpdateChase();
                break;
        }
    }

    void ChangeState(State newState)
    {
        if (currentState == newState) return;

        currentState = newState;
        Debug.Log(name + " -> " + newState);

        switch (newState)
        {
            case State.Patrol:
                patrolWaitTimer = 0f;
                GoToNextPatrolPoint();
                break;

            case State.Investigate:
                agent.SetDestination(investigateTarget);
                break;

            case State.Alarm:
                break;

            case State.Chase:
                if (player != null)
                    agent.SetDestination(player.position);
                break;
        }
    }

    void UpdatePatrol()
    {
        // Si l'ennemi voit le joueur, il lance une alarme
        if (vision != null && vision.canSeePlayer)
        {
            lastKnownPlayerPosition = player.position;
            ChangeState(State.Alarm);
            return;
        }

        // Si l'ennemi entend un bruit, il enquête
        if (hearing != null && hearing.heardSound)
        {
            investigateTarget = hearing.lastHeardPosition;
            hearing.ClearSound();
            ChangeState(State.Investigate);
            return;
        }

        // Gestion de la patrouille
        if (!agent.pathPending && agent.remainingDistance <= patrolPointReachDistance)
        {
            patrolWaitTimer += Time.deltaTime;

            if (patrolWaitTimer >= patrolWaitTime)
            {
                GoToNextPatrolPoint();
                patrolWaitTimer = 0f;
            }
        }
    }

    void UpdateInvestigate()
    {
        // Si l'ennemi voit le joueur pendant l'enquête, il lance une alarme
        if (vision != null && vision.canSeePlayer)
        {
            lastKnownPlayerPosition = player.position;
            ChangeState(State.Alarm);
            return;
        }

        // Si un nouveau bruit est entendu, il met à jour sa destination
        if (hearing != null && hearing.heardSound)
        {
            investigateTarget = hearing.lastHeardPosition;
            hearing.ClearSound();
            agent.SetDestination(investigateTarget);
        }

        // Si l'ennemi arrive à la zone à inspecter, il retourne en patrouille
        if (!agent.pathPending && agent.remainingDistance <= investigateReachDistance)
        {
            ChangeState(State.Patrol);
        }
    }

    void UpdateAlarm()
    {
        // L'ennemi alerte les autres sur la position du joueur
        TryAlert(lastKnownPlayerPosition);

        // Puis il passe lui-même en poursuite
        ChangeState(State.Chase);
    }

    void UpdateChase()
    {
        if (vision != null && vision.canSeePlayer)
        {
            lostSightTimer = 0f;
            lastKnownPlayerPosition = player.position;
            agent.SetDestination(player.position);
        }
        else
        {
            lostSightTimer += Time.deltaTime;

            // S'il perd le joueur, il va inspecter la dernière position connue
            if (lostSightTimer >= loseSightDelay)
            {
                investigateTarget = lastKnownPlayerPosition;
                ChangeState(State.Investigate);
            }
        }
    }

    void GoToNextPatrolPoint()
    {
        if (patrolPoints == null || patrolPoints.Length == 0) return;

        agent.SetDestination(patrolPoints[patrolIndex].position);
        patrolIndex = (patrolIndex + 1) % patrolPoints.Length;
    }

    void TryAlert(Vector3 position)
    {
        if (Time.time - lastAlertTime < alertCooldown)
            return;

        lastAlertTime = Time.time;
        AlertOthers(position);
    }

    void AlertOthers(Vector3 position)
    {
        EnemyFSM[] allEnemies = FindObjectsOfType<EnemyFSM>();

        foreach (EnemyFSM enemy in allEnemies)
        {
            if (enemy == this) continue;
            enemy.OnAlert(position);
        }
    }

    public void OnAlert(Vector3 position)
    {
        // Si cet ennemi est déjà en Chase, on ne l'interrompt pas
        if (currentState == State.Chase)
            return;

        // Si cet ennemi est déjà en Alarm, on évite de relancer la boucle
        if (currentState == State.Alarm)
            return;

        investigateTarget = position;
        ChangeState(State.Investigate);
    }
}