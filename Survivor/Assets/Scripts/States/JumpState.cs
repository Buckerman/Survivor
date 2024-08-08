using Entities.Player;
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
        if (_player.GetComponent<CharacterController>().isGrounded)
        {
            _player.SetState(new LandState());
        }
    }
}
