using System.Collections.Generic;
using UnityEngine;

public class ProjectileObjectPool : MonoBehaviour
{
    [SerializeField] private Transform[] spawnPoints; // เปลี่ยนจากตัวเดียวเป็น Array
    [SerializeField] private List<GameObject> projectilePrefabs;
    [SerializeField] private int initialPoolSize = 10;

    private Dictionary<int, List<GameObject>> projectilePools = new();
    private static ProjectileObjectPool instance;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    public static ProjectileObjectPool GetInstance()
    {
        return instance;
    }

    private void Start()
    {
        for (int i = 0; i < projectilePrefabs.Count; i++)
        {
            projectilePools[i] = new List<GameObject>();
            for (int j = 0; j < initialPoolSize; j++)
            {
                CreateNewProjectile(i);
            }
        }
    }

    private void CreateNewProjectile(int index)
    {
        Transform spawnPoint = GetRandomSpawnPoint();

        GameObject prefab = projectilePrefabs[index];
        GameObject p = Instantiate(prefab, spawnPoint.position, spawnPoint.rotation);

        PooledProjectile pooled = p.GetComponent<PooledProjectile>();
        if (pooled == null)
            pooled = p.AddComponent<PooledProjectile>();

        pooled.prefabIndex = index;

        p.SetActive(false);
        projectilePools[index].Add(p);
    }

    public GameObject Acquire(int index = -1)
    {
        if (projectilePrefabs.Count == 0) return null;

        if (index < 0 || index >= projectilePrefabs.Count)
        {
            index = Random.Range(0, projectilePrefabs.Count);
        }

        if (!projectilePools.ContainsKey(index))
        {
            projectilePools[index] = new List<GameObject>();
        }

        if (projectilePools[index].Count == 0)
        {
            CreateNewProjectile(index);
        }

        GameObject p = projectilePools[index][0];
        projectilePools[index].RemoveAt(0);

        Transform spawnPoint = GetRandomSpawnPoint();
        p.transform.position = spawnPoint.position;
        p.transform.rotation = spawnPoint.rotation;

        p.SetActive(true);
        return p;
    }

    public void Return(GameObject projectile)
    {
        PooledProjectile pooled = projectile.GetComponent<PooledProjectile>();
        if (pooled == null)
        {
            return;
        }

        int index = pooled.prefabIndex;

        if (!projectilePools.ContainsKey(index))
        {
            projectilePools[index] = new List<GameObject>();
        }

        projectile.SetActive(false);
        projectilePools[index].Add(projectile);
    }

    private Transform GetRandomSpawnPoint()
    {
        if (spawnPoints.Length == 0)
        {
            Debug.LogWarning("No spawn points assigned!");
            return transform;
        }

        return spawnPoints[Random.Range(0, spawnPoints.Length)];
    }
}
