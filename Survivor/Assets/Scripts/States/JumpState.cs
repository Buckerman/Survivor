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
        _player.SetAnimation("isJumping", true);
        _player.SetAnimation("isClimbing", false);
        _player.SetAnimation("isRunning", false);
    }

    public void Exit()
    {
        _player.SetAnimation("isJumping", false);
    }

    void IPlayerState.Update()
    {
        HandleInput();
    }

    public void HandleInput()
    {
        if (_player.IsClimbing)
        {
            _player.SetState(new ClimbingState());
        }
        if (Mathf.Abs(Input.GetAxis("Horizontal")) > 0.1f || Mathf.Abs(Input.GetAxis("Vertical")) > 0.1f)
        {
            _player.SetState(new RunningState());
        }
        else
        {
            _player.SetState(new IdleState());
        }
    }
}
