using System.Collections.Generic;
using UnityEngine;

public class LootPool
{
    private List<Loot> lootPrefabs;
    private int poolSize;
    private Queue<Loot> coinPool;
    private Queue<Loot> healthPackPool;
    private Loot coinPrefab;
    private Loot healthPackPrefab;

    public LootPool(List<Loot> lootPrefabs, int poolSize)
    {
        this.lootPrefabs = lootPrefabs;
        this.poolSize = poolSize;

        // Initialize separate pools
        coinPool = new Queue<Loot>();
        healthPackPool = new Queue<Loot>();

        // Find the Coin and HealthPack prefabs
        coinPrefab = lootPrefabs.Find(loot => loot is Coin);
        healthPackPrefab = lootPrefabs.Find(loot => loot is HealthPack);

        // Populate the pools
        PopulatePool(coinPool, coinPrefab);
        PopulatePool(healthPackPool, healthPackPrefab);
    }

    private void PopulatePool(Queue<Loot> pool, Loot prefab)
    {
        for (int i = 0; i < poolSize; i++)
        {
            Loot loot = Object.Instantiate(prefab);
            loot.gameObject.SetActive(false);
            pool.Enqueue(loot);
        }
    }

    public Loot GetLoot(Vector3 position)
    {
        Loot lootToSpawn;
        float randomValue = Random.value;

        if (randomValue <= 0.05f && healthPackPrefab != null)
        {
            lootToSpawn = GetLootFromPool(healthPackPool, healthPackPrefab);
        }
        else
        {
            lootToSpawn = GetLootFromPool(coinPool, coinPrefab);
        }

        lootToSpawn.Initialize(position, this);
        return lootToSpawn;
    }

    private Loot GetLootFromPool(Queue<Loot> pool, Loot prefab)
    {
        if (pool.Count > 0)
        {
            Loot loot = pool.Dequeue();
            loot.gameObject.SetActive(true);
            return loot;
        }
        else
        {
            return Object.Instantiate(prefab);
        }
    }

    public void ReturnLoot(Loot loot)
    {
        if (loot != null && loot.gameObject != null)
        {
            loot.gameObject.SetActive(false);

            if (loot is Coin)
            {
                coinPool.Enqueue(loot);
            }
            else if (loot is HealthPack)
            {
                healthPackPool.Enqueue(loot);
            }
        }
    }
}
