using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class TimeManager : MonoBehaviour
{
    #region Fields

    public float startingTime;
    private float countingTime;

    private Text theText;
   
    public GameObject gameOverScreen;

    private PauseMenu thePauseMenu;

    #endregion

	void Start ()
    {
        countingTime = startingTime;

        theText = GetComponent<Text>();

        thePauseMenu = FindObjectOfType<PauseMenu>();
	}
	
	void Update () 
    {
        if (thePauseMenu.IsPaused) return;
        
        countingTime -= Time.deltaTime;

        if ( countingTime <= 0 )
        {
            //gameOverScreen.SetActive(true);
            //player.gameObject.SetActive(false);
          //  healthManager.KillPlayer();
        }

        theText.text = "" + Mathf.Round(countingTime);
	}

    public void ResetTime()
    {
        countingTime = startingTime;
    }
}
