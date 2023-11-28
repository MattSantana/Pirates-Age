using System.Collections;
using System.Collections.Generic;
using Pirates.Core;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private Transform[] spawningPoints;
    SpawnerObjectsPool objectPooler;
    private float spawnerTime;
    private GameplayObserver gameStatChecker;
    private void Start() {
        objectPooler = SpawnerObjectsPool.Instance;
        gameStatChecker = FindObjectOfType<GameplayObserver>();

        spawnerTime = PlayerPrefs.GetFloat("SpawnerTime");

        if(gameStatChecker.gameHasEnded == false) 
        { 
            InvokeRepeating("SpawningChaser", 0f, spawnerTime);

            InvokeRepeating("SpawningShooter", 0f, spawnerTime);
        }
    }
    public void SpawningChaser()
    {
        for (int i = 0; i < 4; i++)
        {
            int spawnIndex = Random.Range(0, spawningPoints.Length);
            if(gameObject.scene.isLoaded)
            {
                objectPooler.SpawnFromPool("Chaser", spawningPoints[spawnIndex].position, spawningPoints[spawnIndex].rotation);
            }
        }
    }

    public void SpawningShooter()
    {
        for (int i = 0; i < 4; i++)
        {
            int spawnIndex = Random.Range(0, spawningPoints.Length);
            if(gameObject.scene.isLoaded)
            {
                objectPooler.SpawnFromPool("Shooter", spawningPoints[spawnIndex].position, spawningPoints[spawnIndex].rotation);
            }
        }
    }
}
