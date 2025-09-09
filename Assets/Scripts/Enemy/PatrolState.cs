using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : IEnemyState
{
    private bool _isMoving;
    private Vector3 _destination;

    public void EnterState(Enemy enemy)
    {
        enemy.Animator.SetTrigger("PatrolState");
        _isMoving = false;
    }

    public void UpdateState(Enemy enemy)
    {
        if (!_isMoving)
        {
            _isMoving = true;
            int index = UnityEngine.Random.Range(0, enemy.Waypoints.Count);
            _destination = enemy.Waypoints[index].position;
            enemy.NavMeshAgent.destination = _destination;
        }
        else
        {
            if (Vector3.Distance(_destination, enemy.transform.position) <= 0.1)
            {
                _isMoving = false;
            }
        }

        if (Vector3.Distance(enemy.transform.position, enemy.Player.transform.position) < enemy.ChaseDistance)
        {
            enemy.SwitchState(enemy.ChaseState);
        }

    }

    public void ExitState(Enemy enemy)
    {
        Debug.Log("Stop Patrol");
    }
}
