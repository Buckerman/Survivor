using UnityEngine;

public class IceSpikeSpawner
{
    private GameObject iceSpikePrefab;
    public int numberOfSpikes = 7;
    public float radius = 1.5f;

    private void Awake()
    {
        iceSpikePrefab = ResourcesManager.Instance.Load<GameObject>("Prefabs/ActiveAbilities/IceSpikes.prefab");
    }
    void SpawnIceSpikes(Vector3 playerPosition)
    {
        float angleStep = 360f / numberOfSpikes;

        for (int i = 0; i < numberOfSpikes; i++)
        {
            float angle = i * angleStep * Mathf.Deg2Rad;
            Vector3 spikePosition = new Vector3(
                playerPosition.x + Mathf.Cos(angle) * radius,
                playerPosition.y,
                playerPosition.z + Mathf.Sin(angle) * radius
            );
            Quaternion spikeRotation = Quaternion.Euler(0, -i * angleStep, -15);
            GameObject iceSpikeObject = ObjectPooling.Instance.GetObject(iceSpikePrefab);
        }
    }
}
