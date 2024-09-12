using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootDropManager : MonoBehaviour
{
    [Header("Loot Drop Settings")]
    [SerializeField] private List<GameObject> lootPrefabs;
    [SerializeField] private List<float> lootProbabilities;
    [SerializeField] private GameObject defaultLootPrefab;

    private List<float> cumulativeProbability;

    public void Initialize()
    {
        MakeCumulativeProbability(lootProbabilities);
    }

    public void DropLoot(Vector3 position)
    {
        GameObject lootPrefab = GetLootPrefab();
        GameObject lootObject = ObjectPooling.Instance.GetObject(lootPrefab);

        Loot loot = lootObject.GetComponent<Loot>();
        loot.Initialize(position);
    }

    private GameObject GetLootPrefab()
    {
        int index = GetItemByProbability();
        if (index == -1 || index >= lootPrefabs.Count)
        {
            return defaultLootPrefab;
        }
        return lootPrefabs[index];
    }

    public int GetItemByProbability()
    {
        float rnd = Random.Range(0f, 100f);

        for (int i = 0; i < cumulativeProbability.Count; i++)
        {
            if (rnd < cumulativeProbability[i])
            {
                return i;
            }
        }

        return -1;
    }

    private void MakeCumulativeProbability(List<float> probability)
    {
        float cumulativeSum = 0;
        cumulativeProbability = new List<float>();

        for (int i = 0; i < probability.Count; i++)
        {
            cumulativeSum += probability[i];
            cumulativeProbability.Add(cumulativeSum);
        }

        if (cumulativeProbability.Count == 0 || cumulativeSum < 100f)
        {
            cumulativeProbability.Add(100f);
        }
    }
}
