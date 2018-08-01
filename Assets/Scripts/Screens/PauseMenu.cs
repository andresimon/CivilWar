using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour 
{
    private bool isPaused;
    public bool IsPaused { get { return isPaused; } }

    private bool canPause;
    public bool CanPause { get { return canPause; } set { canPause = value;} }

    private bool evaluate;

    public GameObject pauseMenuCanvas;

    public GameObject controlsScreen;

    void Start() 
    {
        canPause = true;
        evaluate = false;
    }

    public void Quit()
    {
        ResumeButton();
        GetComponent<LevelLoader>().LoadLevel();
    }

    public void ResumeButton()
    {
        evaluate = true;
    }

    public void Resume()
    {
        if ( !controlsScreen.GetComponent<ControlsScreen>().IsShowing )
            isPaused = false;
    }

    public void Controls()
    {
        controlsScreen.GetComponent<ControlsScreen>().Show();
    }

    private void Pause()
    {
        canPause = canPause && !controlsScreen.GetComponent<ControlsScreen>().IsShowing;

        if (!canPause) return;

        isPaused = true;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            evaluate = true;
        
        if ( evaluate ) 
        {
            if (isPaused)
                Resume();
            else
                Pause();
            
            Time.timeScale = (isPaused ? 0f : 1f);
            PlayerController.Instance.AllowControl = !isPaused;

            pauseMenuCanvas.SetActive(isPaused);

            evaluate = false;
        }
    }
}
