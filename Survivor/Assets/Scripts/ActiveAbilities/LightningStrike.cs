using QuangDM.Common;
using System.Collections;
using UnityEngine;

public class LightningStrike : MonoBehaviour
{
    private ParticleSystem lightningParticleSystem;
    private void Awake()
    {
        lightningParticleSystem = GetComponent<ParticleSystem>();
    }
    public void Initialize(Vector3 playerPosition, GameObject target, float distance, float damageAmount, float lightningDuration)
    {
        transform.position = new Vector3(playerPosition.x, transform.position.y, playerPosition.z);
        Vector3 direction = (target.transform.position - playerPosition).normalized;
        transform.forward = -direction;

        lightningParticleSystem.GetComponent<ParticleSystemRenderer>().lengthScale = distance / transform.localScale.z;
        lightningParticleSystem.Play();

        target.gameObject.GetComponent<EnemyHealth>().TakeDamage(damageAmount);
        target.gameObject.GetComponent<EnemyController>().StunEnemy(lightningDuration);

        StartCoroutine(DisableAfterParticles());
    }
    private IEnumerator DisableAfterParticles()
    {
        while (!lightningParticleSystem.isStopped)
        {
            yield return null;
        }
        gameObject.SetActive(false);
    }
}