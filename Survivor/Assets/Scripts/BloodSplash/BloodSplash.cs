using QuangDM.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BloodSplash : MonoBehaviour
{
    private ParticleSystem _bloodSystem;
    private BloodSplashPool _bloodSplashPool;
    private float yPositionOffset;

    private void Awake()
    {
        _bloodSystem = GetComponentInChildren<ParticleSystem>();
    }

    public void Initialize(Transform enemyTransform, BloodSplashPool pool)
    {
        if (Math.Abs(yPositionOffset) < Mathf.Epsilon)
        {
            yPositionOffset = transform.position.y;
        }

        transform.rotation = enemyTransform.rotation * Quaternion.Euler(0, 180, 0);
        Vector3 newPosition = new Vector3(enemyTransform.position.x, enemyTransform.position.y + yPositionOffset, enemyTransform.position.z);
        transform.position = newPosition;

        _bloodSplashPool = pool;

        _bloodSystem.Play();

        StartCoroutine(DisableAfterParticles());

        Observer.Instance.AddObserver(EventName.DisableAllBloodSplash, DisableAllBloodSplash);
    }

    private IEnumerator DisableAfterParticles()
    {
        yield return new WaitWhile(() => _bloodSystem.IsAlive(true));
        OnDisable();
    }

    public void DisableAllBloodSplash(object data)
    {
        OnDisable();
    }

    private void OnDisable()
    {
        if (_bloodSplashPool != null)
        {
            Invoke(nameof(RemoveObserver), 0f);
            _bloodSplashPool.ReturnBloodSplash(this);
        }
        _bloodSystem.Stop();
    }

    private void RemoveObserver()
    {
        Observer.Instance.RemoveObserver(EventName.DisableAllBloodSplash, DisableAllBloodSplash);
    }
}