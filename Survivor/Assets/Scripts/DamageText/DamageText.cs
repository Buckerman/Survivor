using QuangDM.Common;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageText : MonoBehaviour
{
    private TextMeshPro _textMeshPro;

    private void Awake()
    {
        _textMeshPro = GetComponentInChildren<TextMeshPro>();
    }

    public void Initialize(Vector3 position)
    {
        float randomX = Random.Range(-1f, 1f);
        Vector3 offset = new Vector3(randomX, 2f, 0f);

        this.transform.position = position + offset;
        Observer.Instance.AddObserver(EventName.DisableAllDamageText, DisableAllDamage);
    }

    public void Setup(int damageAmount, Color color)
    {
        _textMeshPro.text = damageAmount.ToString();
        _textMeshPro.color = color;
    }

    public void DisableAllDamage(object data)
    {
        OnDisable();
    }

    public void Disable()
    {
        OnDisable();
    }
    private void OnDisable()
    {
        Invoke(nameof(RemoveObserver), 0f);
        this.gameObject.SetActive(false);
    }
    private void RemoveObserver()
    {
        Observer.Instance.RemoveObserver(EventName.DisableAllDamageText, DisableAllDamage);
    }
}
