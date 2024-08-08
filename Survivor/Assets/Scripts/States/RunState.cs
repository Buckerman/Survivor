using Entities.Player;
using UnityEngine;

public class RunningState : IPlayerState
{
    private PlayerController _player;

    public void Enter(PlayerController player)
    {
        _player = player;
        _player.SetAnimation("isRunning", true);
    }

    public void Exit()
    {
        _player.SetAnimation("isRunning", false);
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
        else if (Mathf.Abs(Input.GetAxis("Horizontal")) <= 0.1f && Mathf.Abs(Input.GetAxis("Vertical")) <= 0.1f)
        {
            _player.SetState(new IdleState());
        }
        else if (_player.IsJumping)
        {
            _player.SetState(new JumpState());
        }
    }
}
