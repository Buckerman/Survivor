using System.Collections;
using UnityEngine;

public class AbilityManager : MonoBehaviour
{
    private Barrier barrier;
    private bool isBarrier;
    private bool isIceSpikes;

    [Header("Lightning Settings")]
    public float lightningCooldown = 10f;
    public float lightningRadius = 5f;
    public float lightningDamage = 1f;
    public float lightningDuration = 0.5f;

    private float lightningTimer;
    private bool isLightningStrikes = true;

    public void Initialize()
    {
        lightningTimer = lightningCooldown;
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
            // Implement IceSpikes logic
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
}
