using UnityEngine;
using System.Collections;

[RequireComponent (typeof(AudioSource))]

public class VideoPlayer : MonoBehaviour
{
    private MovieTexture movie;
    public MovieTexture Movie 
    {
        get
        {
            return movie;   
        }
    }

    void Awake()
    {
        movie = GetComponent<Renderer>().material.mainTexture as MovieTexture;
    }

    public void Playa()
    {
        movie.Play();
    }

	void Update ()
    {
       
	}
}
