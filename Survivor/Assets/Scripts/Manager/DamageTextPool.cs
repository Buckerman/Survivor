using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTextPool : MonoBehaviour
{
    private DamageText damageTextPrefab;
    private int poolSize;
    private Queue<DamageText> pool;

    public DamageTextPool(DamageText damageTextPrefab, int poolSize)
    {
        this.damageTextPrefab = damageTextPrefab;
        this.poolSize = poolSize;
        pool = new Queue<DamageText>();

        for (int i = 0; i < poolSize; i++)
        {
            DamageText damageText = Object.Instantiate(damageTextPrefab);
            damageText.gameObject.SetActive(false);
            pool.Enqueue(damageText);
        }
    }

    public DamageText GetDamageText()
    {
        if (pool.Count > 0)
        {
            DamageText damageText = pool.Dequeue();
            if (damageText != null && damageText.gameObject != null)
            {
                damageText.gameObject.SetActive(true);
                return damageText;
            }
            else
            {
                return Object.Instantiate(damageTextPrefab);
            }
        }
        else
        {
            return Object.Instantiate(damageTextPrefab);
        }
    }

    public void ReturnDamageText(DamageText damageText)
    {
        if (damageText != null && damageText.gameObject != null)
        {
            damageText.gameObject.SetActive(false);
            pool.Enqueue(damageText);
        }
    }
}
