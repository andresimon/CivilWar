using UnityEngine;
using System.Collections;

public class MeleeState : IEnemyState
{
    private Enemy enemy;

    private float attackTimer;
    private float attackCoolDown = 3f;
    private bool canAttack = true;

    #region IEnemyState implementation
    public void Enter(Enemy enemy)
    {
        this.enemy = enemy;
    }

    public void Execute()
    {
        Attack();

        if (enemy.InRangedRange && !enemy.InMeleeRange)
        {
            if ( enemy.isBoss )
                enemy.ChangeState(new PatrolState());
            else
                enemy.ChangeState(new RangedState());
        }
        else if (enemy.Target == null)
        {
            enemy.ChangeState(new IdleState());
        }
    }

    public void Exit()
    {
    }
    public void OnTriggerEnter(Collider2D other)
    {
    } 
    #endregion

    private void Attack()
    {
        attackTimer += Time.deltaTime;

        if ( attackTimer >= attackCoolDown )
        {
            canAttack = true;

            attackTimer = 0f;
        }

        if ( canAttack )
        {
            canAttack = false;

            enemy.Anim.SetTrigger("Attack");
        }
    }
    
}
