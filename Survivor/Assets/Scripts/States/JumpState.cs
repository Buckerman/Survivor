using Entities.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpState : IPlayerState
{
    private PlayerController _player;

    public void Enter(PlayerController player)
    {
        _player = player;
        _player.SetAnimation("isRunning", false);
        _player.SetAnimation("isJumping", true);
    }

    public void Exit(){ }

    void IPlayerState.Update()
    {
        HandleInput();
    }

    public void HandleInput()
    {
        throw new System.NotImplementedException();
    }
}
