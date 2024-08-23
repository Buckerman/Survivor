using Entities.Player;
using UnityEngine;

public class ClimbingState : IPlayerState
{
    private PlayerController _player;

    public void Enter()
    {
        _player = PlayerController.Instance;
        _player.SetAnimation("isClimbing", true);
        _player.SetAnimation("isRunning", false);
    }

    public void Exit()
    {
        _player.SetAnimation("isClimbing", false);
    }

    public void Update()
    {
        HandleInput();
    }

    public void HandleInput()
    {
        if (!_player.IsClimbing)
        {
            if (Mathf.Abs(Input.GetAxis("Horizontal")) > 0.1f || Mathf.Abs(Input.GetAxis("Vertical")) > 0.1f)
            {
                _player.SetState(new RunningState());
            }
            else _player.SetState(new IdleState());
        }
    }
}