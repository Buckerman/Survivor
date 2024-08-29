using QuangDM.Common;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageText : MonoBehaviour
{
    private TextMeshPro _textMeshPro;
    private Transform _transform;
    private DamageTextPool _damageTextPool;

    private void Awake()
    {
        _textMeshPro = GetComponentInChildren<TextMeshPro>();
    }

    public void Initialize(DamageTextPool pool)
    {
        _damageTextPool = pool;
        Observer.Instance.AddObserver("DisableAllDamageText", DisableAllDamage);
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
        if (_damageTextPool != null)
        {
            Invoke(nameof(RemoveObserver), 0f);
            _damageTextPool.ReturnDamageText(this);
        }
    }
    private void RemoveObserver()
    {
        Observer.Instance.RemoveObserver("DisableAllDamageText", DisableAllDamage);
    }
}
