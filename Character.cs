using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    [SerializeField]
    protected float movmentSpeed = 10f;
    [SerializeField]
    protected GameObject knife;
    [SerializeField]
    protected Transform knifePos;
    [SerializeField]
    protected int health = 30;
    [SerializeField]
    protected EdgeCollider2D sword;
    [SerializeField]
    protected List<string> damagingObjects;

    protected bool facingRight = true;
    protected float horizontal = 0f;
    public bool Attack { get; set; }
    public bool Melee { get; set; }
    public Animator Animator { get; private set; }

    public EdgeCollider2D Sword { get { return sword; } }

    public abstract bool IsDead { get; }
    public bool TakingDamage { get; set; }

    // Start is called before the first frame update
    public virtual void Start()
    {
        Animator = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public abstract IEnumerator TakeDamage();

    protected virtual void ThrowKnife(int value)
    {
        GameObject aux;
        if (facingRight)
        {
            aux = Instantiate(knife, knifePos.position, Quaternion.Euler(new Vector3(0, 0, -90)));
            aux.GetComponent<Knife>().Initialized(Vector2.right);
        }
        else
        {
            aux = Instantiate(knife, knifePos.position, Quaternion.Euler(new Vector3(0, 0, 90)));
            aux.GetComponent<Knife>().Initialized(Vector2.left);
        }
    }

    public void ChangeDirection()
    {
        facingRight = !facingRight;
        transform.localScale = new Vector3(-transform.localScale.x, 1, 1);
    }

    protected virtual void OnTriggerEnter2D(Collider2D collider)
    {
        if (damagingObjects.Contains(collider.tag))
        {
            StartCoroutine(TakeDamage());
        }
    }


    public void MeleeAttack()
    {
        sword.enabled = true;
    }

}
