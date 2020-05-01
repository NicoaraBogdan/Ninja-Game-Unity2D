using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedState : IEnemyState
{
    private Enemy enemy;

    private float throwTimer = 0f;
    private float throwCooldown = 6f;
    private bool canThrow = true;
    public void Enter(Enemy enemy)
    {
        this.enemy = enemy;
    }

    public void Execute()
    {
        Throw();

        if (enemy.InMeleeRange)
        {
            enemy.ChangeState(new MeleeState());
        }

        if (!enemy.Attack && !enemy.InMeleeRange)
        {
            if (enemy.Target != null)
            {
                enemy.Move();
            }
            else
            {
                enemy.ChangeState(new PatrolState());
            }
        }
    }

    public void Exit()
    {
    }

    public void OnTriggerEnter(Collider2D other)
    {
    }

    private void Throw()
    {
        throwTimer += Time.deltaTime;
        if(throwTimer >= throwCooldown)
        {
            canThrow = true;
            throwTimer = 0;
        }

        if(canThrow)
        {
            canThrow = false;
            enemy.Animator.SetTrigger("throw");
        }
    }

}
