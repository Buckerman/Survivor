using UnityEngine;

public class IceRing: MonoBehaviour
{
    public int numberOfSpikes = 7;
    public float radius = 1.5f;
    public float cooldown = 20f;

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
            GameObject iceSpikeObject = ObjectPooling.Instance.GetObject(ResourcesManager.Instance.Load<GameObject>("Prefabs/ActiveAbilities/IceSpike"));
        }
    }
    void PerformAction()
    {

    }
}
