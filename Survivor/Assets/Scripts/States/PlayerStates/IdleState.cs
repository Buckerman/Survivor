using UnityEngine;

public class IdleState : IPlayerState
{
    private PlayerController _playerController;

    public void Enter()
    {
        _playerController = Player.Instance.GetComponent<PlayerController>();
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
        if (_playerController.IsClimbing)
        {
            _playerController.SetState(new ClimbingState());
        }
        else if (_playerController.magnitude > 0.1f)
        {
            _playerController.SetState(new RunningState());
        }
    }
}