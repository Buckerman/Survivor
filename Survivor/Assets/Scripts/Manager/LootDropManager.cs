using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootDropManager : MonoBehaviour
{
    [Header("Loot Drop Settings")]
    [SerializeField] private List<Loot> lootPrefabs;
    [SerializeField] private int lootPoolSize = 10;
    private LootPool _lootPool;

    public void Initialize()
    {
        _lootPool = new LootPool(lootPrefabs, lootPoolSize);
    }

    public void DropLoot(Vector3 position)
    {
        _lootPool.GetLoot(position);
    }
}
