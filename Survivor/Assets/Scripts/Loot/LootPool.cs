using System.Collections.Generic;
using UnityEngine;

public class LootPool
{
    private List<Loot> lootPrefabs;
    private int poolSize;
    private Queue<Loot> pool;
    private Loot coinPrefab;
    private Loot healthPackPrefab;

    public LootPool(List<Loot> lootPrefabs, int poolSize)
    {
        this.lootPrefabs = lootPrefabs;
        this.poolSize = poolSize;
        pool = new Queue<Loot>();

        // Dynamically find the Coin and HealthPack prefabs
        coinPrefab = lootPrefabs.Find(loot => loot is Coin);
        healthPackPrefab = lootPrefabs.Find(loot => loot is HealthPack);

        for (int i = 0; i < poolSize; i++)
        {
            Loot loot = Object.Instantiate(lootPrefabs[Random.Range(0, lootPrefabs.Count)]);
            loot.gameObject.SetActive(false);
            pool.Enqueue(loot);
        }
    }

    public Loot GetLoot(Vector3 position)
    {
        Loot lootToSpawn;

        // 25% chance to drop a health pack
        if (Random.value <= 0.25f && healthPackPrefab != null)
        {
            lootToSpawn = GetLootFromPoolOrInstantiate(healthPackPrefab);
        }
        else
        {
            lootToSpawn = GetLootFromPoolOrInstantiate(coinPrefab);
        }

        lootToSpawn.Initialize(position, this);
        return lootToSpawn;
    }

    private Loot GetLootFromPoolOrInstantiate(Loot lootPrefab)
    {
        if (pool.Count > 0)
        {
            Loot loot = pool.Dequeue();
            if (loot != null && loot.gameObject != null)
            {
                loot.gameObject.SetActive(true);
                return loot;
            }
        }
        return Object.Instantiate(lootPrefab);
    }

    public void ReturnLoot(Loot loot)
    {
        if (loot != null && loot.gameObject != null)
        {
            loot.gameObject.SetActive(false);
            pool.Enqueue(loot);
        }
    }
}
