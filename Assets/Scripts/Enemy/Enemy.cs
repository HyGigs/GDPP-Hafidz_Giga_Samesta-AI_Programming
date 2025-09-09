using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] public List <Transform> Waypoints = new List<Transform>();
    [SerializeField] public float ChaseDistance;
    [SerializeField] public Player Player;

    [HideInInspector] public NavMeshAgent NavMeshAgent;

    private IEnemyState _currentState;
    public PatrolState PatrolState = new PatrolState();
    public ChaseState ChaseState = new ChaseState();
    public RetreatState RetreatState = new RetreatState();

    private void Awake()
    {
        NavMeshAgent = GetComponent<NavMeshAgent>();
        _currentState = PatrolState;
        _currentState.EnterState(this);
    }

    private void Start()
    {
        if (Player != null)
        {
            Player.OnPowerUpStart += StartRetreating;
            Player.OnPowerUpStop += StopRetreating;
        }
    }


    private void Update()
    {
        if (_currentState != null)
        {
            _currentState.UpdateState(this);
        }
    }

    public void SwitchState(IEnemyState state)
    {
        _currentState.ExitState(this);
        _currentState = state;
        _currentState.EnterState(this);
    }

    private void StartRetreating()
    {
        SwitchState(RetreatState);
    }

    private void StopRetreating()
    {
        SwitchState(PatrolState);
    }

}