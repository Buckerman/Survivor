using UnityEngine;

public class IceSpikeSpawner : MonoBehaviour
{
    public GameObject iceSpikePrefab;  
    public int numberOfSpikes = 8;
    public float radius = 5f;

    void SpawnIceSpikes()
    {
        Vector3 playerPosition = transform.position;

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
            Instantiate(iceSpikePrefab, spikePosition, spikeRotation);
        }
    }
    private void Start()
    {
        SpawnIceSpikes();
    }
}
