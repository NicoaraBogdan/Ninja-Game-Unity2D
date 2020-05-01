using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : IEnemyState
{
    private float idleTimer = 0;
    private float idleDuration = 3;
    private Enemy enemy;
    public void Enter(Enemy enemy)
    {
        this.enemy = enemy;
    }

    public void Execute()
    {
        Idle();

        if (enemy.Target != null)
        {
            enemy.ChangeState(new PatrolState());
        }
    }

    public void Exit()
    {
    }

    public void OnTriggerEnter(Collider2D other)
    {
    }

    private void Idle()
    {
        idleTimer += Time.deltaTime;
        enemy.Animator.SetFloat("speed", 0);

        if(idleTimer >= idleDuration)
        {
            enemy.ChangeState(new PatrolState());
            idleTimer = 0;
        }
    }
}
