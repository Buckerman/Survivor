using Entities.Player;
using UnityEngine;

public class IdleState : IPlayerState
{
    private PlayerController _player;

    public void Enter(PlayerController player)
    {
        _player = player;
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
        else if(Mathf.Abs(Input.GetAxis("Horizontal")) > 0.1f || Mathf.Abs(Input.GetAxis("Vertical")) > 0.1f)
        {
            _player.SetState(new RunningState());
        }
        else if (_player.IsJumping)
        {
            _player.SetState(new JumpState());
        }
    }
}