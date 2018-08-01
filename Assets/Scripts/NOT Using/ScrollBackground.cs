using UnityEngine;
using System.Collections;

public class ScrollBackground : MonoBehaviour 
{
    public float speed;

	// Use this for initialization
	void Start () {
	
	}
	
	void Update () 
    {
        GetComponent<Renderer>().material.mainTextureOffset = new Vector2((Time.time * speed) % 1, 0f);
	}
}
