using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour 
{
    public GameObject controlsScreen;

    public int playerLives;

    void Start()
    {
        ClearSingletons();
    }

    public void NewGame()
    {
        PlayerPrefs.SetInt("PlayerCurrentLives", playerLives);

//        PlayerPrefs.SetInt("CurrentPlayerScore", 0);

        GetComponent<LevelLoader>().LoadLevel();
    }

    public void Controls()
    {
        controlsScreen.GetComponent<ControlsScreen>().Show();
    }

    public void QuitGame()
    {
        Debug.LogWarning("Game Exited");
        Application.Quit();
    }

    private void ClearSingletons()
    {
        PlayerController player = FindObjectOfType<PlayerController>();
        if (player) Destroy(player.gameObject);

        GameManager gameManager = GameObject.FindObjectOfType<GameManager>();
        if (gameManager) Destroy(gameManager.gameObject);
    }

}
