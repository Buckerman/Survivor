using Entities.Player;
using UnityEngine;

public class InAirState : IPlayerState
{
    private PlayerController _player;

    public void Enter()
    {
        _player = PlayerController.Instance;
        _player.SetAnimation("isInAir", true);
    }

    public void Exit()
    {
        _player.SetAnimation("isInAir", false);
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