using UnityEngine;
using System.Collections;

public class Lvl01_End_Manager : MonoBehaviour 
{
    private float waitBeforeLoad = 2f;

    private bool finished;

    void Update () 
    {
        if ( finished )
        {
            if (waitBeforeLoad <= 0)
                GetComponent<LevelLoader>().LoadLevel();
            else
                waitBeforeLoad -= Time.deltaTime;
        }
        else
        {
            finished = GetComponent<ExplosionSpawner>().Done;
        }
    }
}
