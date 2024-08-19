using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageText : MonoBehaviour
{
    private TextMesh _textMesh; 

    private void Awake()
    {
        _textMesh = GetComponent<TextMesh>();
    }
    public void Setup(int damageAmount)
    {
        _textMesh.text = damageAmount.ToString();
    }
}
