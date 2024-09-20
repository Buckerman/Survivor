using System.Collections;
using UnityEngine;

public class AbilityManager : MonoBehaviour
{
    private Barrier barrier;
    private bool isBarrier;

    [Header("Lightning Settings")]
    public float lightningCooldown = 10f;
    public float lightningRadius = 5f;
    public float lightningDamage = 1f;
    public float lightningDuration = 0.5f;

    private float lightningTimer;
    private bool isLightningStrikes = true;

    [Header("IceSpikes Settings")]
    public float iceSpikesCooldown = 20f;
    public float iceSpikesRadius = 1.5f;
    public float iceSpikesDamage = 1f;
    public float iceSpikesNumber = 7f;
    public float iceSpikesDuration = 1f;

    private float iceSpikesTimer;
    private bool isIceSpikes = true;

    public void Initialize()
    {
        lightningTimer = lightningCooldown;
        iceSpikesTimer = iceSpikesCooldown;
        barrier = Player.Instance.gameObject.GetComponentInChildren<Barrier>();
    }

    private void Update()
    {
        if (isBarrier)
        {
            barrier.ActivateBarrier();
        }

        if (isIceSpikes)
        {
            iceSpikesTimer -= Time.deltaTime;

            if (iceSpikesTimer <= 0f)
            {
                ActivateIceSpikes();
                iceSpikesTimer = iceSpikesCooldown;
            }
        }

        if (isLightningStrikes)
        {
            lightningTimer -= Time.deltaTime;

            if (lightningTimer <= 0f)
            {
                ActivateLightningStrikes();
                lightningTimer = lightningCooldown;
            }
        }
    }
    private void ActivateLightningStrikes()
    {
        Collider[] hitColliders = Physics.OverlapSphere(Player.Instance.transform.position, lightningRadius, LayerMask.GetMask("Enemy"));

        foreach (Collider hitCollider in hitColliders)
        {
            float distance = Vector3.Distance(Player.Instance.transform.position, hitCollider.transform.position);
            if (distance <= lightningRadius && hitCollider != null)
            {
                GameObject lightningBoltObject = ObjectPooling.Instance.GetObject(ResourcesManager.Instance.Load<GameObject>("Prefabs/ActiveAbilities/LightningBolt"));
                LightningStrike lightningBolt = lightningBoltObject.GetComponent<LightningStrike>();

                lightningBolt.Initialize(Player.Instance.transform.position, hitCollider.gameObject, distance, lightningDamage, lightningDuration);
            }
        }
    }
    private void ActivateIceSpikes()
    {
        float angleStep = 360f / iceSpikesNumber;

        for (int i = 0; i < iceSpikesNumber; i++)
        {
            float angle = i * angleStep * Mathf.Deg2Rad;

            GameObject iceSpikeObject = ObjectPooling.Instance.GetObject(ResourcesManager.Instance.Load<GameObject>("Prefabs/ActiveAbilities/IceSpike"));
            IceRing iceRing = iceSpikeObject.GetComponent<IceRing>();

            iceSpikeObject.transform.rotation = Quaternion.Euler(0, -i * angleStep, -15);
            iceRing.Initialize(Player.Instance.transform.position,angle, iceSpikesRadius);
        }
    }
}
