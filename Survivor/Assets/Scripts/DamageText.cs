using QuangDM.Common;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
public class DamageText : MonoBehaviour
{
    private TextMesh _textMesh;
    private Transform _transform;
    private DamageTextPool _damageTextPool;

    private void Awake()
    {
        _textMesh = GetComponent<TextMesh>();
    }
    private void Start()
    {
        Observer.Instance.AddObserver("DisableAllDamageText", DisableAllText);
    }
    public void Initialize(DamageTextPool pool)
    {
        _damageTextPool = pool;
    }
    public void Setup(int damageAmount, Color color)
    {
        _textMesh.text = damageAmount.ToString();
        _textMesh.color = color;
    }
    private void DisableAllText(object data)
    {
        OnDisable();
    }
    private void OnDisable()
    {
        if (_damageTextPool != null)
        {
            _damageTextPool.ReturnDamageText(this);
        }
    }
}
