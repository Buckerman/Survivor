using System.Collections;
using UnityEngine;

public class AbilityManager : MonoBehaviour
{
    private float barrierTimer;
    private float lightningTimer;
    private float iceSpikesTimer;

    [Header("Barrier Settings")]
    public float barrierCooldown = 30f;
    private bool isBarrier;
    private Barrier barrier;

    [Header("Lightning Settings")]
    public float lightningCooldown = 10f;
    public float lightningRadius = 5f;
    public float lightningDamage = 1f;
    public float lightningStunDuration = 0.5f;

    [Header("IceSpikes Settings")]
    public float iceSpikesCooldown = 25f;
    public float iceSpikesRadius = 1.5f;
    public float iceSpikesDamage = 2f;
    public float iceSpikesNumber = 7f;
    public float iceSpikesSlowDuration = 1.5f;
    public float iceSpikesSlowAmount = 0.8f;

    public void Initialize()
    {
        lightningTimer = lightningCooldown;
        iceSpikesTimer = iceSpikesCooldown;
        barrierTimer = barrierCooldown;
        barrier = Player.Instance.GetComponentInChildren<Barrier>();
    }

    private void Update()
    {
        barrierTimer = Mathf.Max(0, barrierTimer - Time.deltaTime);
        iceSpikesTimer = Mathf.Max(0, iceSpikesTimer - Time.deltaTime);
        lightningTimer = Mathf.Max(0, lightningTimer - Time.deltaTime);

        if (barrierTimer <= 0f)
        {
            barrier.ActivateBarrier();
            barrierTimer = barrierCooldown;
        }

        if (iceSpikesTimer <= 0f)
        {
            ActivateIceSpikes();
            iceSpikesTimer = iceSpikesCooldown;
        }
        if (lightningTimer <= 0f)
        {
            ActivateLightningStrikes();
            lightningTimer = lightningCooldown;
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

                lightningBolt.Initialize(Player.Instance.transform.position, hitCollider.gameObject, distance, lightningDamage);
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
            iceRing.Initialize(Player.Instance.transform.position, angle, iceSpikesRadius);
        }
    }
}
