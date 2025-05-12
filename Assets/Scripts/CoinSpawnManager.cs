using UnityEngine;
using System.Collections.Generic;

public class CoinSpawnManager : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField] private GameObject[] coinPrefabs;      // เหรียญหลายชนิด
    [SerializeField] private Transform[] spawnPoints;        // จุดเกิดเหรียญ
    [SerializeField] private float spawnInterval = 2f;       // เวลาระหว่างการเกิด
    [SerializeField] private int initialPoolSize = 10;       // ขนาดเริ่มต้นของ pool

    private Dictionary<int, Queue<GameObject>> coinPools = new();

    private float timer;

    private void Start()
    {
        // สร้าง Pool สำหรับแต่ละ prefab
        for (int i = 0; i < coinPrefabs.Length; i++)
        {
            coinPools[i] = new Queue<GameObject>();
            for (int j = 0; j < initialPoolSize; j++)
            {
                GameObject coin = CreateNewCoin(i);
                coin.SetActive(false);
                coinPools[i].Enqueue(coin);
            }
        }

        timer = spawnInterval;
    }

    private void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            SpawnCoin();
            timer = spawnInterval;
        }
    }

    private void SpawnCoin()
    {
        int prefabIndex = Random.Range(0, coinPrefabs.Length);
        GameObject coin = GetCoinFromPool(prefabIndex);
        if (coin == null) return;

        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        coin.transform.position = spawnPoint.position;
        coin.transform.rotation = spawnPoint.rotation;
        coin.SetActive(true);
    }

    private GameObject CreateNewCoin(int index)
    {
        GameObject coin = Instantiate(coinPrefabs[index]);
        CoinPooledMarker marker = coin.GetComponent<CoinPooledMarker>();
        if (marker == null)
        {
            marker = coin.AddComponent<CoinPooledMarker>();
        }
        marker.prefabIndex = index;
        marker.pool = this;
        return coin;
    }

    private GameObject GetCoinFromPool(int index)
    {
        if (!coinPools.ContainsKey(index))
        {
            coinPools[index] = new Queue<GameObject>();
        }

        if (coinPools[index].Count == 0)
        {
            return CreateNewCoin(index);
        }

        return coinPools[index].Dequeue();
    }

    public void ReturnCoinToPool(GameObject coin)
    {
        CoinPooledMarker marker = coin.GetComponent<CoinPooledMarker>();
        if (marker == null)
        {
            Debug.LogWarning("Returned coin missing CoinPooledMarker");
            Destroy(coin);
            return;
        }

        coin.SetActive(false);
        coinPools[marker.prefabIndex].Enqueue(coin);
    }

    // Marker component for identifying pool origin
    private class CoinPooledMarker : MonoBehaviour
    {
        public int prefabIndex;
        public CoinSpawnManager pool;

        private void OnDisable()
        {
            if (gameObject.activeInHierarchy == false)
            {
                // optional: auto-return after X sec or via collision
            }
        }

        public void ReturnToPool()
        {
            pool?.ReturnCoinToPool(gameObject);
        }
    }
}
