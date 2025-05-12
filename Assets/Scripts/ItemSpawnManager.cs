using UnityEngine;
using System.Collections.Generic;

public class ItemSpawnManager : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField] private GameObject[] itemPrefabs;      // ไอเทมหลายชนิด
    [SerializeField] private Transform[] spawnPoints;       // จุดเกิดไอเทม
    [SerializeField] private float spawnInterval = 2f;      // เวลาระหว่างการเกิด
    [SerializeField] private int initialPoolSize = 10;      // ขนาดเริ่มต้นของ pool

    private Dictionary<int, Queue<GameObject>> itemPools = new();

    private float timer;

    private void Start()
    {
        for (int i = 0; i < itemPrefabs.Length; i++)
        {
            itemPools[i] = new Queue<GameObject>();
            for (int j = 0; j < initialPoolSize; j++)
            {
                GameObject item = CreateNewItem(i);
                item.SetActive(false);
                itemPools[i].Enqueue(item);
            }
        }

        timer = spawnInterval;
    }

    private void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            SpawnItem();
            timer = spawnInterval;
        }
    }

    private void SpawnItem()
    {
        int prefabIndex = Random.Range(0, itemPrefabs.Length);
        GameObject item = GetItemFromPool(prefabIndex);
        if (item == null) return;

        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        item.transform.position = spawnPoint.position;
        item.transform.rotation = spawnPoint.rotation;
        item.SetActive(true);
    }

    private GameObject CreateNewItem(int index)
    {
        GameObject item = Instantiate(itemPrefabs[index]);
        ItemPooledMarker marker = item.GetComponent<ItemPooledMarker>();
        if (marker == null)
        {
            marker = item.AddComponent<ItemPooledMarker>();
        }
        marker.prefabIndex = index;
        marker.pool = this;
        return item;
    }

    private GameObject GetItemFromPool(int index)
    {
        if (!itemPools.ContainsKey(index))
        {
            itemPools[index] = new Queue<GameObject>();
        }

        if (itemPools[index].Count == 0)
        {
            return CreateNewItem(index);
        }

        return itemPools[index].Dequeue();
    }

    public void ReturnItemToPool(GameObject item)
    {
        ItemPooledMarker marker = item.GetComponent<ItemPooledMarker>();
        if (marker == null)
        {
            Debug.LogWarning("Returned item missing ItemPooledMarker");
            Destroy(item);
            return;
        }

        item.SetActive(false);
        itemPools[marker.prefabIndex].Enqueue(item);
    }

    // Marker component for identifying pool origin
    private class ItemPooledMarker : MonoBehaviour
    {
        public int prefabIndex;
        public ItemSpawnManager pool;

        public void ReturnToPool()
        {
            pool?.ReturnItemToPool(gameObject);
        }
    }
}
