using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    // public List<GameObject> obstaclePrefabs;
    public Vector3 spawnPos = new(25, 0, 0);

    public float startDelay = 1.5f;
    public float repeatRate = 1.5f;


    void Start()
    {
        InvokeRepeating(nameof(SpawnObstacle), startDelay, repeatRate);
    }

    void SpawnObstacle()
    {
        // if (obstaclePrefabs.Count == 0) return; 

        // int index = Random.Range(0, obstaclePrefabs.Count);
        // Instantiate(obstaclePrefabs[index], spawnPos, obstaclePrefabs[index].transform.rotation);
        ProjectileObjectPool.GetInstance().Acquire();
    }
}
