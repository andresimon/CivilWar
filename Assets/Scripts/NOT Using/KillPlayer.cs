using UnityEngine;
using System.Collections;

public class KillPlayer : MonoBehaviour 
{

    public GameManager levelManager;

	void Start () 
    {
        levelManager = FindObjectOfType<GameManager>();
	}
	
    void OnTriggerEnter2D(Collider2D other)
    {
        //if (other.name == "Player")
          //  levelManager.RespawnPlayer();
    }
}
