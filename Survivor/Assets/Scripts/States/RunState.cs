using Entities.Player;
using UnityEngine;

public class RunningState : IPlayerState
{
    private PlayerController _player;

    public void Enter(PlayerController player)
    {
        _player = player;
        _player.SetAnimation("isRunning", true);
        _player.SetAnimation("isJumping", false);
        _player.SetAnimation("isClimbing", false);
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
        if (Mathf.Abs(Input.GetAxis("Horizontal")) <= 0.1f && Mathf.Abs(Input.GetAxis("Vertical")) <= 0.1f)
        {
            _player.SetState(new IdleState());
        }
    }
}
