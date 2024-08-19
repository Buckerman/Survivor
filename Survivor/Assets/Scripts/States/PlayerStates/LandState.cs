using Entities.Player;
using UnityEngine;

public class LandState : IPlayerState
{
    private PlayerController _player;

    public void Enter(PlayerController player)
    {
        _player = player;
        _player.SetAnimation("isLanding", true);
    }

    public void Exit()
    {
        _player.SetAnimation("isLanding", false);
    }

    public void Update()
    {
        HandleInput();
    }

    public void HandleInput()
    {
        if (_player.IsClimbing)
        {
            _player.SetState(new ClimbingState());
        }
        else if (_player.IsJumping)
        {
            _player.SetState(new JumpState());
        }
        else if (_player.magnitude > 0.1f)
        {
            _player.SetState(new RunningState());
        }
        else if (_player.magnitude <= 0.1f)
        {
            _player.SetState(new IdleState());
        }

    }
}