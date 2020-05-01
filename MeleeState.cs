using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeState : IEnemyState
{
    private Enemy enemy;

    private float attackCooldown = 2f;
    private float attackTimer;
    private bool canAttack = true;
    public void Enter(Enemy enemy)
    {
        this.enemy = enemy;
    }

    public void Execute()
    {
        Attack();

        if (!enemy.InMeleeRange && enemy.InThrowRange)
        {
            enemy.ChangeState(new RangedState());
        }
        else if (!enemy.Target)
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

    private void Attack()
    {
        attackTimer += Time.deltaTime;
        enemy.Melee = true;

        if(attackTimer >= attackCooldown)
        {
            canAttack = true;
            attackTimer = 0f;
        }

        if(canAttack)
        {
            canAttack = false;
            enemy.Animator.SetTrigger("attack");
        }
    }
}
