using UnityEngine;
using System.Collections.Generic;

public class BuildingAndPlatformScatterer : MonoBehaviour
{
    [System.Serializable]
    public class BuildingType
    {
        public GameObject prefab;
        public int amount;
        public string sizeCategory;
    }

    public BuildingType[] buildingTypes;
    public GameObject platformPrefab;
    public int platformAmount;
    public float scatterRange = 450f;
    public LayerMask buildingLayerMask;

    public Transform hugeBuildingsParent;
    public Transform bigBuildingsParent;
    public Transform mediumBuildingsParent;
    public Transform smallBuildingsParent;
    public Transform platformsParent;

    private List<Vector3> buildingPositionList = new List<Vector3>();

    [ContextMenu("Scatter Buildings and Platforms")]
    public void ScatterBuildingsAndPlatforms()
    {
        // Clear existing buildings and platforms
        ClearExistingObjects(hugeBuildingsParent);
        ClearExistingObjects(bigBuildingsParent);
        ClearExistingObjects(mediumBuildingsParent);
        ClearExistingObjects(smallBuildingsParent);
        ClearExistingObjects(platformsParent);
        buildingPositionList.Clear();

        // Sort and instantiate buildings by category
        SpawnBuildings();

        // Scatter platforms based on building positions
        SpawnPlatforms();
    }

    private void SpawnBuildings()
    {
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
                    if (buildingType.sizeCategory == "Huge" || buildingType.sizeCategory == "Big")
                    {
                        buildingPositionList.Add(randomPosition);
                    }
                }
                else
                {
                    i--; // Retry if overlap detected
                }
            }
        }
    }

    private void SpawnPlatforms()
    {
        const int maxGlobalAttempts = 1000; // Total maximum attempts to find valid positions
        int platformsPlaced = 0;
        int globalAttempts = 0;

        HashSet<Vector3> usedPositions = new HashSet<Vector3>();

        while (platformsPlaced < platformAmount && globalAttempts < maxGlobalAttempts)
        {
            foreach (Vector3 buildingPosition in buildingPositionList)
            {
                if (platformsPlaced >= platformAmount)
                    break;

                Vector3 possiblePosition = LookForPlatformPosition(buildingPosition);
                globalAttempts++;

                if (!IsOverlapping(possiblePosition, platformPrefab) && !usedPositions.Contains(possiblePosition))
                {
                    Instantiate(platformPrefab, possiblePosition, Quaternion.identity, platformsParent);
                    usedPositions.Add(possiblePosition);
                    platformsPlaced++;
                }

                if (globalAttempts >= maxGlobalAttempts)
                {
                    Debug.LogWarning("Max attempts reached. Could not place all platforms.");
                    break;
                }
            }
        }

        if (platformsPlaced < platformAmount)
        {
            Debug.LogWarning($"Only {platformsPlaced} platforms were placed out of the desired {platformAmount}.");
        }
    }

    private Vector3 LookForPlatformPosition(Vector3 buildingPosition)
    {
        // Generate a random direction
        Vector3 randomDirection = Random.insideUnitCircle.normalized;

        Vector3 platformPosition = buildingPosition + randomDirection * 17f;

        // Keep the Y position the same as the prefab's Y position
        platformPosition.y = platformPrefab.transform.position.y;

        return platformPosition;
    }


    private void ClearExistingObjects(Transform parent)
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
            default:
                return null;
        }
    }
}
