using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DebuffType
{
    SLOW
}
public class EnemyAuras : MonoBehaviour
{
    [SerializeField] public ParticleSystem freezeAura;

    public void PlayEffect(DebuffType debuffType)
    {
        switch (debuffType)
        {
            case DebuffType.SLOW:
                freezeAura.Play();
                break;
        }
    }

    public void StopEffect(DebuffType debuffType)
    {
        switch (debuffType)
        {
            case DebuffType.SLOW:
                freezeAura.Stop();
                break;
        }
    }
}
