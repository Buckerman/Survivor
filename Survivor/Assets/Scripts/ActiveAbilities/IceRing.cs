using System.Collections;
using UnityEngine;

public class IceRing : MonoBehaviour
{
    ParticleSystem iceSpikeParticleSystem;

    private void Awake()
    {
        iceSpikeParticleSystem = GetComponent<ParticleSystem>();
    }
    public void Initialize(Vector3 playerPosition, float angle, float radius)
    {
        transform.position = new Vector3(
            playerPosition.x + Mathf.Cos(angle) * radius,
            playerPosition.y,
            playerPosition.z + Mathf.Sin(angle) * radius
        );
        iceSpikeParticleSystem.Play();
        StartCoroutine(DisableAfterParticles());
    }
    private IEnumerator DisableAfterParticles()
    {
        while (!iceSpikeParticleSystem.isStopped)
        {
            yield return null;
        }
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.gameObject.GetComponent<EnemyHealth>().TakeDamage(GameManager.Instance.GetComponent<AbilityManager>().iceSpikesDamage);
        }
    }
}
