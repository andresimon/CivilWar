using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour 
{
    #region Fields

    public Transform spawnSpot;
    public int spawnQuantity;
    public float spawnRate;
    private float spawnRateTimer;

    private int enemiesSpawned;

    public GameObject enemyPrefab;
    protected GameObject enemy;

    public bool changeDirection;

    public bool Done
    {
        get
        {
            return enemiesSpawned == spawnQuantity;
        }
    }

    #endregion

    public virtual void SpawnEnemy()
    { 
        if ( enemiesSpawned < spawnQuantity )
        {
            if ( spawnRateTimer >= spawnRate   )
            {
                enemy = Instantiate(enemyPrefab, spawnSpot.position, spawnSpot.rotation) as GameObject;

                if ( changeDirection )
                    enemy.GetComponent<Enemy>().ChangeDirection();

                enemiesSpawned++;

                spawnRateTimer = 0;
            }
            spawnRateTimer += Time.deltaTime;
        }
    }


}
