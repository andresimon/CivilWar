using UnityEngine;
using System.Collections;

public class Lvl01_Intro_Manager : MonoBehaviour
{
    [SerializeField] private GameObject lagosCanvas;
    private float lagosCanvasTime = 5f;
    private float timer = 0f;

    private float waitBeforeLoad = 2f;

    private bool finished;

	void Update () 
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            waitBeforeLoad = 0f;
            finished = true;
        }
        
        if ( finished )
        {
            if (waitBeforeLoad <= 0)
                GetComponent<LevelLoader>().LoadLevel();
            else
                waitBeforeLoad -= Time.deltaTime;
        }
        else
        {
            lagosCanvas.GetComponent<Canvas>().enabled = (timer > 1 && timer <= lagosCanvasTime);
            timer += Time.deltaTime;
        }
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if ( other.tag == "Crossbones")
        {
            finished = true;
        }

        Destroy(other.gameObject);
    }
}
