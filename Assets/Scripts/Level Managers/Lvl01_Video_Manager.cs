using UnityEngine;
using System.Collections;

public class Lvl01_Video_Manager : MonoBehaviour 
{
    [SerializeField] private GameObject videoPlayer;
    private MovieTexture movie;

    [SerializeField] private GameObject toBeContinued;

    private float waitBeforeReturn = 4f;
    private float waitTimer;

	void Start () 
    {
        movie = videoPlayer.GetComponent<VideoPlayer>().Movie;
        movie.Play();
    }
	
	void Update ()
    {
        if ( movie.isPlaying )
        {
            if ( Input.GetKeyDown(KeyCode.Escape) )
            {
                movie.Stop();
                toBeContinued.SetActive(true);   
            }    
        }
        else
        {
            if (waitBeforeReturn <= 0)
            {
                GetComponent<LevelLoader>().LoadLevel();
            }
            else
            {
                if ( !toBeContinued.activeSelf ) toBeContinued.SetActive(true);
                waitBeforeReturn -= Time.deltaTime;
            }
        }

	}
}
