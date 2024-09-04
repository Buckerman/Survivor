using QuangDM.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private float maxHealth = 12f;
    private int _currentHealth;

    private void Start()
    {
        _currentHealth = (int)maxHealth;
    }
    public void TakeDamage(float damage)
    {
        _currentHealth -= (int)damage;
        Observer.Instance.Notify(EventName.DamageReceived, (this, damage));

        if (_currentHealth <= 0)
            Die();
    }

    private void Die()
    {
        Observer.Instance.Notify(EventName.BloodSpawn, this.transform);
        Observer.Instance.Notify(EventName.DropLoot, this.transform.position);
        this.gameObject.SetActive(false);
    }
}
