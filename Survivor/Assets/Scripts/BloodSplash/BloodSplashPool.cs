using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodSplashPool
{
    private BloodSplash bloodSplashPrefab;
    private int poolSize;
    private Queue<BloodSplash> pool;

    public BloodSplashPool(BloodSplash bloodSplashPrefab, int poolSize)
    {
        this.bloodSplashPrefab = bloodSplashPrefab;
        this.poolSize = poolSize;
        pool = new Queue<BloodSplash>();

        for (int i = 0; i < poolSize; i++)
        {
            BloodSplash bloodSplash = Object.Instantiate(bloodSplashPrefab);
            bloodSplash.gameObject.SetActive(false);
            pool.Enqueue(bloodSplash);
        }
    }

    public BloodSplash GetBloodSplash(Transform enemyTransform)
    {
        BloodSplash bloodSplash;

        if (pool.Count > 0)
        {
            bloodSplash = pool.Dequeue();
            bloodSplash.gameObject.SetActive(true);
        }
        else
        {
            bloodSplash = Object.Instantiate(bloodSplashPrefab);
        }

        bloodSplash.Initialize(enemyTransform, this);
        return bloodSplash;
    }


    public void ReturnBloodSplash(BloodSplash bloodSplash)
    {
        if (bloodSplash != null && bloodSplash.gameObject != null)
        {
            bloodSplash.gameObject.SetActive(false);
            pool.Enqueue(bloodSplash);
        }
    }
}
