using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Lvl01_a_Manager : MonoBehaviour 
{
    private int enemiesRemaining;
    public Text enemiesRemainingText;

    private float playerLeftBound = -28.1f;
    private float playerRightBound = 148;
    private float playerMaxRightBound = 190;

    private float cameraLeftBound;

    private bool finished;
    private float waitBeforeLoad = 0f;


    void Start () 
    {
        cameraLeftBound = Camera.main.GetComponent<CameraBounds>().XMin;

        PlayerController.Instance.GetComponent<PlayerBounds>().SetBounds(playerLeftBound, playerRightBound);
	}
	
	void Update () 
    {
        UpdateEnemiesCounter();

        if ( finished )
        {
            if (waitBeforeLoad <= 0)
                GetComponent<LevelLoader>().LoadLevel();
            else
                waitBeforeLoad -= Time.deltaTime;
        }
        else
        {
            enemiesRemaining =  FindObjectsOfType<Enemy>().Length;
            if (enemiesRemaining == 0) ChangeBounds();
        }
    }

    private void ChangeBounds()
    {
        PlayerController.Instance.GetComponent<PlayerBounds>().SetBounds(playerLeftBound, playerMaxRightBound);

        Camera.main.GetComponent<CameraBounds>().SetBounds(cameraLeftBound, 170);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if ( other.tag == "Player")
        {
            PlayerController.Instance.AllowControl = false;
            finished = true;
        }
    }

    private void UpdateEnemiesCounter()
    {
        enemiesRemainingText.text = "Enemies Remaining: " + enemiesRemaining;
    }
}
