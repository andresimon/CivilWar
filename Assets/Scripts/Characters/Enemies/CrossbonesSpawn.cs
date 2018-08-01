using UnityEngine;
using System.Collections;

public class CrossbonesSpawn : EnemySpawner
{
    void Update()
    {
        if ( GetComponent<EnemySpawner>().Done )
           SpawnEnemy();
    }

    public override void SpawnEnemy()
    {
        base.SpawnEnemy();
    }
}
