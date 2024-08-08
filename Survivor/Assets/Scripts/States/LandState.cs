using Entities.Player;
using UnityEngine;

public class LandState : IPlayerState
{
    private PlayerController _player;

    public void Enter(PlayerController player)
    {
        _player = player;
        _player.SetAnimation("isLanding", true);
        _player.SetAnimation("isRunning", false);
        _player.SetAnimation("isClimbing", false);
        _player.SetAnimation("isJumping", false);
    }

    public void Exit() { }

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
    }
}