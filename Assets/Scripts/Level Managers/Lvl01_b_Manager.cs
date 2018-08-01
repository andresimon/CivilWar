using UnityEngine;
using System.Collections;

public class Lvl01_b_Manager : MonoBehaviour
{
    public GameObject currentCheckpoint;

    private float playerLeftBound = -28.1f;
    private float playerRightBound = 117.2f;

    private bool finished;
    private float waitBeforeLoad = 0f;
    private float waitBeforeMove = 3f;

    public GameObject boss;
    public GameObject wandaSpell;

    public Transform finishSpot;

    private int stage = 0;
    /*
     * 0 -> normal
     * 1 -> dead
     * 2 -> move
     * 3 -> finish level
     */

    void Awake()
    {
        GameManager.Instance.currentCheckpoint = currentCheckpoint;   
    }

    void Start () 
    {
        PlayerController.Instance.GetComponent<PlayerBounds>().SetBounds(playerLeftBound, playerRightBound);

    }

    void Update () 
    {
        if (stage == 0 && boss.GetComponent<Enemy>().IsDead) stage = 1;
        
        switch ( stage )
        {
            case 0:
                break;
            case 1:
                crossbonesDead();
                break;
            case 2:
                crossbonesMove();
                break;
            case 3:
                endLevel();
                break;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if ( other.tag == "Crossbones")
        {
            stage = 3;
            FindObjectOfType<CameraBounds>().Target = null;
            Destroy(other.gameObject.transform.parent.gameObject);
        }
    }

    private void crossbonesDead()
    {
        wandaSpell.SetActive(true);
        wandaSpell.transform.localPosition = new Vector2(boss.transform.localPosition.x, boss.transform.localPosition.y - 2);
        wandaSpell.transform.parent = boss.transform;

        FindObjectOfType<CameraBounds>().Target = boss.transform;

        boss.GetComponent<Rigidbody2D>().gravityScale = 0f;
         
        stage = 2;
    }

    private void crossbonesMove()
    {
        if (waitBeforeMove <= 0)
            boss.transform.position = Vector3.MoveTowards(boss.transform.position, finishSpot.position, Time.deltaTime * 20);
        else
            waitBeforeMove -= Time.deltaTime;
    }

    private void endLevel()
    {  
        if (waitBeforeLoad <= 0)
            GetComponent<LevelLoader>().LoadLevel();
        else
            waitBeforeLoad -= Time.deltaTime;
    }
}
