using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    private IEnemyState currentState;
    public GameObject Target { get; set; }

    private float meleeRange = 3;
    private float throwRange = 15;

    public override bool IsDead
    {
        get { return health <= 0; }
    }

    public bool InMeleeRange
    {
        get 
        {
            if (Target != null)
            {
                return Vector2.Distance(transform.position, Player.Instance.transform.position) <= meleeRange;
            }
            return false;
        }
    }

    public bool InThrowRange
    {
        get
        {
            if (Target != null)
            {
                return Vector2.Distance(transform.position, Player.Instance.transform.position) <= throwRange;
            }
            return false;
        }
    }

    public override void Start()
    {
        base.Start();
        currentState = new IdleState();   
    }

    void Update()
    {
        currentState.Enter(this);

        if (!IsDead)
        {
            if (!TakingDamage)
            {
                currentState.Execute();

            }
            LookAtTarget();
        }
    }

    public void ChangeState(IEnemyState newState)
    {
        if(currentState != null)
        {
            currentState.Exit();
        }
        currentState = newState;
        currentState.Enter(this);
    }

    public void Move()
    {
        Animator.SetFloat("speed", 1);
        transform.Translate(GetDirection() * movmentSpeed * Time.deltaTime);
    }

    private Vector2 GetDirection()
    {
        return facingRight ? Vector2.right : Vector2.left;
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        currentState.OnTriggerEnter(collision);
    }

    void LookAtTarget()
    {
        float xDir;

        if(Target != null && !Attack)
        {
            xDir = Target.transform.position.x - transform.position.x;

            if(xDir < 0 && facingRight || xDir > 0 && !facingRight)
            {
                ChangeDirection();
            }
        }
    }

    public override IEnumerator TakeDamage()
    {
        health -= 10;

        if (!IsDead)
        {
            Animator.SetTrigger("damage");
        }
        else
        {
            Animator.SetTrigger("dead");
            yield return new WaitForSeconds(5);
            Destroy(gameObject);
        }
    }
}
