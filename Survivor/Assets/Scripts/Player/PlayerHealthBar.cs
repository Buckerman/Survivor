using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour
{
    private Slider _slider;
    void Awake()
    {
        _slider = GetComponentInChildren<Slider>();
    }
    private void LateUpdate()
    {
        transform.rotation = Camera.main.transform.rotation;
    }
    public void UpdateHealthBar(float currentHealth, float maxHealth)
    {
        _slider.value = currentHealth / maxHealth;
    }
}
