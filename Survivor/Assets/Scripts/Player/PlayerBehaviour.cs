using UnityEngine;
using UnityEngine.UI;

public class PlayerBehaviour : MonoBehaviour
{
    public int maxHealth = 100;
    private int _currentHealth;
    private PlayerHealthBar _healthBar;

    private void Start()
    {
        _currentHealth = maxHealth;
        _healthBar = GetComponentInChildren<PlayerHealthBar>();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            TakeDamage(10);
        }
    }

    private void TakeDamage(int damage)
    {
        _currentHealth -= damage;
        _healthBar.UpdateHealthBar(_currentHealth, maxHealth);
        if (_currentHealth <= 0) Die();
    }

    private void Die()
    {
        GameManager.Instance.EndGame();
    }
}
