using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionTrigger : MonoBehaviour
{

    private BoxCollider2D player;

    [SerializeField]
    private BoxCollider2D platformCollider;
    [SerializeField]
    private BoxCollider2D platformTrigger;

    void Start()
    {
        player = GameObject.Find("Player").GetComponent<BoxCollider2D>();
        Physics2D.IgnoreCollision(platformCollider, platformTrigger, true);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.name == "Player")
        {
            Physics2D.IgnoreCollision(player, platformCollider, true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.name == "Player")
        {
            Physics2D.IgnoreCollision(platformCollider, player, false);
        }
    }
}
