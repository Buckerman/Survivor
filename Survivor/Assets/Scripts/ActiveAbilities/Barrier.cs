using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrier : MonoBehaviour
{
    public float cooldown = 30f;
    private ParticleSystem barrierParticleSystem;

    private void Awake()
    {
        barrierParticleSystem = GetComponent<ParticleSystem>();
    }
    public void ActivateBarrier()
    {
        barrierParticleSystem.Play();
        Player.Instance.GetComponent<PlayerHealth>().isBarrier = true;
    }
    public void DisableBarrier()
    {
        barrierParticleSystem.Stop();
    }
}
