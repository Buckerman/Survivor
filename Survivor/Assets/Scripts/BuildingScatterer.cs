using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Unity.AI.Navigation;

public class BuildingScatterer : MonoBehaviour
{
    [System.Serializable]
    public class BuildingType
    {
        public GameObject prefab;
        public int amount;
        public string sizeCategory;
    }

    public BuildingType[] buildingTypes;
    public float scatterRange = 400f;
    public LayerMask buildingLayerMask;

    public Transform hugeBuildingsParent;
    public Transform bigBuildingsParent;
    public Transform mediumBuildingsParent;
    public Transform smallBuildingsParent;
    public Transform tinyBuildingsParent;
    public Transform platformParent;

    [ContextMenu("Scatter Buildings")]
    public void ScatterBuildings()
    {
        // Clear existing buildings
        ClearExistingBuildings(hugeBuildingsParent);
        ClearExistingBuildings(bigBuildingsParent);
        ClearExistingBuildings(mediumBuildingsParent);
        ClearExistingBuildings(smallBuildingsParent);
        ClearExistingBuildings(tinyBuildingsParent);
        ClearExistingBuildings(platformParent);


        // Sort and instantiate buildings by category
        foreach (BuildingType buildingType in buildingTypes)
        {
            Transform parent = GetParentByCategory(buildingType.sizeCategory);

            for (int i = 0; i < buildingType.amount; i++)
            {
                Vector3 randomPosition = GetRandomPosition(buildingType.prefab);
                Quaternion randomRotation = Quaternion.Euler(0, Random.Range(0, 360), 0);

                if (!IsOverlapping(randomPosition, buildingType.prefab))
                {
                    Instantiate(buildingType.prefab, randomPosition, randomRotation, parent);
                }
                else
                {
                    i--; // Retry if overlap detected
                }
            }
        }
    }

    private void ClearExistingBuildings(Transform parent)
    {
        if (parent != null)
        {
            while (parent.childCount > 0)
            {
                DestroyImmediate(parent.GetChild(0).gameObject);
            }
        }
    }

    private Vector3 GetRandomPosition(GameObject prefab)
    {
        float x = Random.Range(-scatterRange, scatterRange);
        float z = Random.Range(-scatterRange, scatterRange);
        float y = prefab.transform.position.y; // Keep prefab's original Y position
        return new Vector3(x, y, z);
    }

    private bool IsOverlapping(Vector3 position, GameObject prefab)
    {
        Collider[] colliders = Physics.OverlapBox(position, prefab.transform.localScale / 2, Quaternion.identity, buildingLayerMask);
        return colliders.Length > 0;
    }

    private Transform GetParentByCategory(string category)
    {
        switch (category)
        {
            case "Huge":
                return hugeBuildingsParent;
            case "Big":
                return bigBuildingsParent;
            case "Medium":
                return mediumBuildingsParent;
            case "Small":
                return smallBuildingsParent;
            case "Tiny":
                return tinyBuildingsParent;
            case "Platform":
                return platformParent;
            default:
                return null;
        }
    }
}
