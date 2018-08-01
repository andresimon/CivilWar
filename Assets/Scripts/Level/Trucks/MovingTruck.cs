using UnityEngine;
using System.Collections;

public class MovingTruck : MonoBehaviour
{
    public GameObject truck;
    public Transform endPoint;
   
    public float moveSpeed;
    private bool moving;

    private Animator anim;

    private bool doorIsOpen;

    public AudioClip movingAudio;

	void Start () 
    {
        anim = GetComponentInChildren<Animator>();
        moving = moveSpeed > 0 ? true : false;

        if (!moving)
            doorIsOpen = true;
	}
	
	void Update () 
    {
        if ( moving )
        {
            truck.transform.localPosition = Vector3.MoveTowards(truck.transform.localPosition, endPoint.localPosition, Time.deltaTime * moveSpeed);

            if (truck.transform.localPosition == endPoint.localPosition)
                moving = false;
        }

        anim.SetBool("Moving", moving);

        HandleAnimations();

        if (!moving && doorIsOpen)
            GetComponent<EnemySpawner>().SpawnEnemy();
	}

    private void HandleAnimations()
    {
        AnimatorStateInfo animState = anim.GetCurrentAnimatorStateInfo(0);

        if ( !doorIsOpen && !moving )
        {
            anim.SetBool("DoorIsOpen", doorIsOpen);
            if ( animState.IsName("OpenDoor") && animState.length > animState.normalizedTime )
                doorIsOpen = true;
        }
    }

}
