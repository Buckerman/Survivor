using QuangDM.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAuras : MonoBehaviour
{
    [SerializeField] public ParticleSystem levelUpAura;
    [SerializeField] public ParticleSystem healAura;
    [SerializeField] public ParticleSystem speedBuffAura;

    private void Start()
    {
        Observer.Instance.AddObserver(EventName.PlayerLevelUp, PlayerLevelUp);
    }
    private void PlayerLevelUp(object data)
    {
        levelUpAura.Play();
    }
}
