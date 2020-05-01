using UnityEngine;

public class IgnoreColision : MonoBehaviour
{
    [SerializeField]
    private GameObject other;

    private void Awake()
    {
        Physics2D.IgnoreCollision(gameObject.GetComponent<Collider2D>(),
                                  other.GetComponent<Collider2D>());
    }
}
