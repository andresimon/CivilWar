using UnityEngine;
using System.Collections;

public class HealthPickup : MonoBehaviour 
{
    public int healthRegenAmount;

    public AudioClip soundEffect;

    void OnTriggerEnter2D(Collider2D other)
    {
        if ( other.tag == "Player")
        {
            PlayerController.Instance.GiveHealth(healthRegenAmount);
    
            AudioSource.PlayClipAtPoint(soundEffect, transform.position);

            Destroy(gameObject);
        }
    }
}
