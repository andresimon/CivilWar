using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LifeManager : MonoBehaviour 
{
    #region Fields

    private int lifeCounter;

    private Text theText;

    public GameObject gameOverScreen;

    public string mainMenu;

    public float waitAfterGameOver;

    #endregion

    void Start ()  
    {
        theText = GetComponent<Text>();

        lifeCounter = PlayerPrefs.GetInt("PlayerCurrentLives");
	} 
	
	void Update () 
    {
      
        if (lifeCounter < 0)
        {
            gameOverScreen.SetActive(true);
            PlayerController.Instance.AllowControl = false;
            FindObjectOfType<PauseMenu>().CanPause = false;
        }
        
        theText.text = "x " + lifeCounter;

        if ( gameOverScreen.activeSelf )
        {
            waitAfterGameOver -= Time.deltaTime;
        }

        if ( waitAfterGameOver < 0 )
        {
            SceneManager.LoadScene(mainMenu);
        }
	}

    public void GiveLife()
    {
        lifeCounter++;
        PlayerPrefs.SetInt("PlayerCurrentLives", lifeCounter);
    }

    public void TakeLife()
    {
        lifeCounter--;
        PlayerPrefs.SetInt("PlayerCurrentLives", lifeCounter);

    }
}
