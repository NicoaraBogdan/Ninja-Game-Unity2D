using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{

    private static Player instance;

    public static Player Instance 
    {
        get
        {
            if(instance == null)
            {
                instance = FindObjectOfType<Player>();
            }
            return instance;
        }
    }

    [SerializeField]
    private float jumpForce = 200f;
    [SerializeField]
    private Transform[] groundPoints;
    [SerializeField]
    private LayerMask whatIsGround;
    [SerializeField]
    private SpriteRenderer spriteRenderer;

    private Transform respawnPoint;
    
    public Rigidbody2D Rigidbody { get; set;}
    public bool Slide { get; set; }
    public bool OnGround { get; set; }
    public bool Jump { get; set; }

    public override bool IsDead
    {
        get
        {
            Animator.SetBool("respawn", false);
            return health <= 0; 
        }
    }

    private float immortalTimer = 3f;
    private bool immortal = false;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        Rigidbody = GetComponent<Rigidbody2D>();
        horizontal = Input.GetAxis("Horizontal");
        respawnPoint = GameObject.Find("RespawnPoint").transform;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        StartCoroutine(Blinck());
        StartCoroutine(Respawn());
        Debug.Log(IsDead);
        if (!IsDead)
        {
            if (!TakingDamage)
            {
                horizontal = Input.GetAxis("Horizontal");
                OnGround = IsGrounded();
                Flip(horizontal);
                HandleLayers();
                HandleMovment(horizontal);
            }
        }
    }
    private void Update()
    {
        if (!IsDead)
        {
            HandleInputs();
        }
    }

    private void HandleMovment(float horizontal)
    {
        if(Rigidbody.velocity.y < 0)
        {
            Animator.SetBool("land", true);
        }

        if(!Attack && !Slide)
        {
            Rigidbody.velocity = new Vector2(horizontal * movmentSpeed, Rigidbody.velocity.y);
        }
        else if (Attack && OnGround)
        {
            Rigidbody.velocity = Vector2.zero;
        }

        if(Jump && OnGround && !Animator.GetBool("land"))
        {
            Rigidbody.AddForce(new Vector2(0, jumpForce));
        }

        Animator.SetFloat("speed", Mathf.Abs(horizontal));
    }

    private void Flip(float horizontal)
    {
        if(((horizontal < 0 && facingRight) || (horizontal > 0 && !facingRight)) && !Slide && !Attack)
        {
            ChangeDirection();
        }
    }

    private void HandleInputs()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Animator.SetTrigger("attack");
            Melee = true;
        }

        if(Input.GetKeyDown(KeyCode.LeftShift))
        {
            Animator.SetTrigger("slide");
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            Animator.SetTrigger("jump");
        }

        if(Input.GetKeyDown(KeyCode.E))
        {
            Animator.SetTrigger("throw");
        }
    }

    protected override void ThrowKnife(int value)
    {
        if ((!OnGround && value == 1) || (OnGround && value == 0))
        {
            base.ThrowKnife(value);
        }
    }
    private void HandleLayers()
    {
        if (OnGround)
            Animator.SetLayerWeight(1, 0);
        else
            Animator.SetLayerWeight(1, 1);
    }

    private bool IsGrounded()
    {
        if (Rigidbody.velocity.y <= 0)
            foreach(Transform point in groundPoints)
            {
                Collider2D[] colliers = Physics2D.OverlapCircleAll(point.position, 0.02f, whatIsGround);

                for (int i = 0; i < colliers.Length; i++)

                    if (colliers[i].gameObject != gameObject)
                        return true;
            }
        return false;
    }

    private IEnumerator Respawn()
    {
        if (transform.position.y < -10)
        {
            yield return new WaitForSeconds(1f);
            transform.position = new Vector2(respawnPoint.position.x, respawnPoint.position.y);
            Rigidbody.velocity = Vector2.zero;
        }

        else if(IsDead)
        {
            yield return new WaitForSeconds(2f);
            transform.position = new Vector2(respawnPoint.position.x, respawnPoint.position.y);
            Rigidbody.velocity = Vector2.zero;
            Animator.SetBool("respawn", true);
            health = 10;
        }
    }
    private IEnumerator Blinck()
    {
        if (immortal)
        {
            spriteRenderer.enabled = false;
            yield return new WaitForSeconds(0.1f);
            spriteRenderer.enabled = true;
            yield return new WaitForSeconds(0.1f);
        }
    }

    public override IEnumerator TakeDamage()
    {
        if (!immortal && !IsDead)
        {
            health -= 10;
            Animator.SetLayerWeight(1, 0);

            if (!IsDead)
            {
                Animator.SetTrigger("damage");
                Rigidbody.velocity = Vector2.zero;
                immortal = true;
                yield return new WaitForSeconds(immortalTimer);
                immortal = false;
            }
        }

        if (IsDead)
        {
            Animator.SetTrigger("dead");
        }
    }

    protected override void OnTriggerEnter2D(Collider2D collider)
    {
        base.OnTriggerEnter2D(collider);
    }
}
