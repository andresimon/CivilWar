using UnityEngine;
using System.Collections;

public class CameraSwapping : MonoBehaviour 
{

    [SerializeField] private Camera rooftopCam;
    [SerializeField] private Camera groundCam;

    private PlayerController player;

	void Start () 
    {
        rooftopCam.enabled = true;
        groundCam.enabled = false;
	}
	
    void OnTriggerEnter2D(Collider2D other)
    { 
        if (other.tag == "Player")
        { 
            rooftopCam.enabled = false;
            groundCam.enabled = true;

            PlayerController.Instance.transform.position = GameManager.Instance.currentCheckpoint.transform.position;
        }
    }

}
