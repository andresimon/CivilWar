using UnityEngine;
using System.Collections;

public class RangedState : IEnemyState
{
    private Enemy enemy;

    private float shotTimer;
    private float shotCoolDown = 3f;
    private bool canShot = true;

    #region IEnemyState implementation
    public void Enter(Enemy enemy)
    {
        this.enemy = enemy;
    }

    public void Execute()
    {
        if ( !enemy.isBoss)
            Shot();

        if (enemy.InMeleeRange)
        {
            enemy.ChangeState(new MeleeState());
        }
        else if (enemy.Target != null)
        {
            enemy.Move();
        }       
        else
            enemy.ChangeState(new IdleState());
    }

    public void Exit()
    {
    }
    public void OnTriggerEnter(Collider2D other)
    {
    } 
    #endregion

    private void Shot()
    {
        shotTimer += Time.deltaTime;

        if ( shotTimer >= shotCoolDown )
        {
            canShot = true;

            shotTimer = 0f;
        }

        if ( canShot )
        {
            canShot = false;

            enemy.Anim.SetTrigger("RangedAttack");
        }
    }
}
