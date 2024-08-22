using QuangDM.Common;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private float maxHealth = 100f;
    private int _currentHealth;
    private PlayerHealthBar _healthBar;

    private void Start()
    {
        _currentHealth = (int)maxHealth;
        _healthBar = GetComponentInChildren<PlayerHealthBar>();
    }

    public void TakeDamage(float damage)
    {
        _currentHealth -= (int)damage;
        Observer.Instance.Notify("DamageReceived", (this, damage));
        _healthBar.UpdateHealthBar(_currentHealth, maxHealth);

        if (_currentHealth <= 0)
            Die();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            TakeDamage(5f);
        }
    }

    private void Die()
    {
        Observer.Instance.Notify("DisableAllEnemies");
        Observer.Instance.Notify("DisableAllDamageText");

        GameManager.Instance.EndGame();
    }
}
