﻿using QuangDM.Common;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private float maxHealth = 100f;
    private int _currentHealth;
    private PlayerHealthBar _healthBar;

    private void Awake()
    {
        _healthBar = GetComponentInChildren<PlayerHealthBar>();
    }
    private void Start()
    {
        ResetHealth();
    }
    public void ResetHealth()
    {
        _currentHealth = (int)maxHealth;
        _healthBar.UpdateHealthBar(_currentHealth, maxHealth);
    }

    public void TakeDamage(float damage)
    {
        _currentHealth -= (int)damage;
        Observer.Instance.Notify(EventName.DamageReceived, (this, damage));
        _healthBar.UpdateHealthBar(_currentHealth, maxHealth);

        if (_currentHealth <= 0)
            Die();
    }

    public void Heal(float amount)
    {
        _currentHealth += (int)amount;
        if (_currentHealth >= maxHealth) _currentHealth = (int)maxHealth;
        _healthBar.UpdateHealthBar(_currentHealth, maxHealth);
    }

    private void Die()
    {
        Observer.Instance.Notify(EventName.DisableAllEnemies);
        Observer.Instance.Notify(EventName.DisableAllDamageText);

        GameManager.Instance.EndGame();
    }
}