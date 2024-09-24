using QuangDM.Common;
using System.Collections;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using static Unity.IO.LowLevel.Unsafe.AsyncReadManagerMetrics;

public class AbilityManager : MonoBehaviour
{
    private float barrierTimer;
    private float lightningTimer;
    private float iceSpikesTimer;

    public bool isBarrier;
    private bool isLightning;
    private bool isIceSpikes;

    [Header("Barrier Settings")]
    public float barrierCooldown = 20f;
    private Barrier barrier;

    [Header("Lightning Settings")]
    public float lightningCooldown = 5f;
    public float lightningRadius = 5f;
    public float lightningDamage = 1f;
    public float lightningStunDuration = 0.5f;

    [Header("IceSpikes Settings")]
    public float iceSpikesCooldown = 15f;
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

        if (isBarrier && barrierTimer <= 0f)
        {
            barrier.ActivateBarrier();
            barrierTimer = barrierCooldown;
        }

        if (isIceSpikes && iceSpikesTimer <= 0f)
        {
            IceSpikes();
            iceSpikesTimer = iceSpikesCooldown;
        }

        if (isLightning && lightningTimer <= 0f)
        {
            LightningStrikes();
            lightningTimer = lightningCooldown;
        }
    }
    private void LightningStrikes()
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
    private void IceSpikes()
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
    public void ActivateBarrierFlag()
    {
        isBarrier = true;
        CloseAbilityMenu();
    }
    public void ActivateLightningFlag()
    {
        isLightning = true;
        CloseAbilityMenu();
    }
    public void ActivateIceSpikesFlag()
    {
        isIceSpikes = true;
        CloseAbilityMenu();
    }
    private void CloseAbilityMenu()
    {
        Observer.Instance.Notify(EventName.SetAbilityMenu,false);
    }
}
