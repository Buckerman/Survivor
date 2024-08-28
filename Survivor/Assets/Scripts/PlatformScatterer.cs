using QuangDM.Common;
using System.Collections.Generic;
using UnityEngine;

public class PlatformScatterer : MonoBehaviour
{
    public GameObject prefab;
    public int amount;
    public Transform platformsParent;
    public LayerMask buildingLayerMask;

    void Start()
    {
        Observer.Instance.AddObserver("ScatterPlatforms", ScatterPlatforms);
    }

    private void ScatterPlatforms(object data)
    {
        SpawnPlatforms(data);
    }

    private void SpawnPlatforms(object data)
    {
        List<Vector3> buildingPositions = (List<Vector3>)data;

        foreach (Vector3 buildingPosition in buildingPositions)
        {
            for (int i = 0; i < amount; i++)
            {
                Vector3 possiblePosition = LookForPosition(buildingPosition);

                if (!IsOverlapping(possiblePosition, prefab))
                {
                    Instantiate(prefab, possiblePosition, Quaternion.identity, platformsParent);
                }
                else
                {
                    i--; // Retry if overlap detected
                }
            }
        }
    }

    private Vector3 LookForPosition(Vector3 buildingPosition)
    {
        // Calculate a random direction from the building
        Vector3 direction = Random.insideUnitSphere;
        direction.y = 0; // Keep the platform on the same horizontal plane

        // Normalize the direction and multiply by 11f to get the exact distance
        direction.Normalize();
        Vector3 platformPosition = buildingPosition + direction * 11f;

        // Ensure the platform maintains the prefab's original Y position
        float y = prefab.transform.position.y;
        platformPosition.y = y;

        return platformPosition;
    }

    private bool IsOverlapping(Vector3 position, GameObject prefab)
    {
        Collider[] colliders = Physics.OverlapBox(position, prefab.transform.localScale / 2, Quaternion.identity, buildingLayerMask);
        return colliders.Length > 0;
    }
}
