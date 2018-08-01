using UnityEngine;
using System.Collections;

public class LifePickup : MonoBehaviour
{
    public AudioClip soundEffect;

    void OnTriggerEnter2D(Collider2D other)
    {
        if ( other.tag == "Player")
        {
            FindObjectOfType<LifeManager>().GiveLife();

            AudioSource.PlayClipAtPoint(soundEffect, transform.position);

            Destroy(gameObject);
        }
    }   
}
