using UnityEngine;
using System.Collections;

public class HurtEnemyOnContact : MonoBehaviour 
{
    public int damageToGive;

    public bool countAsCombo;

    private GameManager gameManager;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            if ( countAsCombo )
                gameManager.setEnemyBeingHitted(other.name);
        }
    }

}
