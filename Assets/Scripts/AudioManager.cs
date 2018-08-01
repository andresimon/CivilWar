using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour 
{
    public void PlayAudioClip(AudioClip clip)
    {
        GetComponent<AudioSource>().Stop();
        GetComponent<AudioSource>().clip = clip;
        GetComponent<AudioSource>().Play();
    }

    public void StopAudioClip()
    {
        GetComponent<AudioSource>().Stop();
    }

}
