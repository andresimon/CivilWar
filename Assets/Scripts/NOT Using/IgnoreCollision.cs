using UnityEngine;
using System.Collections;

public class IgnoreCollision : MonoBehaviour
{
    [SerializeField] private Collider2D[] others;

    private void Awake()
    {
        foreach ( Collider2D theCollider in others )
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), theCollider, true);
    }

}
