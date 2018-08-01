using UnityEngine;
using System.Collections;

public class PatrolState : IEnemyState
{
    private Enemy enemy;

    private float patrolTimer;
    private float patrolDuration;

    #region IEnemyState implementation
    public void Enter(Enemy enemy)
    {
        patrolDuration = UnityEngine.Random.Range(3, 5) + 1;

        this.enemy = enemy;
    }

    public void Execute()
    {
        Patrol();

        enemy.Move();

        if ( enemy.Target != null && enemy.InRangedRange )
        {
            enemy.ChangeState(new RangedState());
        }
    }

    public void Exit()
    {
    }

    public void OnTriggerEnter(Collider2D other)
    {
        if (other.tag == "Shield")
            enemy.Target = PlayerController.Instance.gameObject;
    }
    #endregion

    private void Patrol()
    {
        patrolTimer += Time.deltaTime;

        if ( patrolTimer >= patrolDuration )
            enemy.ChangeState(new IdleState());
    }
}
