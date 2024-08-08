using Entities.Player;
using UnityEngine;

public class JumpState : IPlayerState
{
    private PlayerController _player;

    public void Enter(PlayerController player)
    {
        _player = player;
        _player.SetAnimation("isStanding", false);
        _player.SetAnimation("isJumping", true);
        _player.SetAnimation("isClimbing", false);
        _player.SetAnimation("isRunning", false);
        _player.SetAnimation("isInAir", false);
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
        if (!_player.GetComponent<CharacterController>().isGrounded)
        {
            _player.SetState(new InAirState());
        }
    }
}
