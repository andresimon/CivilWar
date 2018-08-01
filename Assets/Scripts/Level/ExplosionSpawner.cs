using UnityEngine;
using System.Collections;

public class ExplosionSpawner : MonoBehaviour
{

    #region Fields

    public Transform spawnSpot;
    public int spawnQuantity;
    public float spawnRate;
    private float spawnRateTimer;

    private int explosionsSpawned;

    public GameObject explosionPrefab;
    protected GameObject explosion;

    public GameObject firePrefab;

    public float displacement;

    public bool Done
    {
        get
        {
            return explosionsSpawned == spawnQuantity;
        }
    }

    #endregion

    void Update()
    {
        SpawnEnemy();
    }

    public virtual void SpawnEnemy()
    { 
        if ( explosionsSpawned < spawnQuantity )
        {
            if ( spawnRateTimer >= spawnRate   )
            {
                Instantiate(explosionPrefab, spawnSpot.position, spawnSpot.rotation);
                Instantiate(firePrefab, spawnSpot.position, spawnSpot.rotation);

                explosionsSpawned++;

                spawnRateTimer = 0;

                spawnSpot.transform.Translate(Vector3.right * displacement, Camera.main.transform);
            }
            spawnRateTimer += Time.deltaTime;
        }
    }
}
