using Entities.Player;
using UnityEngine;

public class IdleState : IPlayerState
{
    private PlayerController _player;

    public void Enter()
    {
        _player = PlayerController.Instance;
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
        else if (_player.magnitude > 0.1f)
        {
            _player.SetState(new RunningState());
        }
    }
}