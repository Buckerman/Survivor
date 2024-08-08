using Entities.Player;
using UnityEngine;

public class InAirState : IPlayerState
{
    private PlayerController _player;

    public void Enter(PlayerController player)
    {
        _player = player;
        _player.SetAnimation("isStanding", false);
        _player.SetAnimation("isJumping", false);
        _player.SetAnimation("isClimbing", false);
        _player.SetAnimation("isRunning", false);
        _player.SetAnimation("isInAir", true);
        _player.SetAnimation("isLanding", false);
    }

    public void Exit()
    {
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
        else if (_player.GetComponent<CharacterController>().isGrounded)
        {
            _player.SetState(new LandState());
        }
    }
}