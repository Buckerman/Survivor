using UnityEngine;

public class RunningState : IPlayerState
{
    private PlayerController _playerController;

    public void Enter()
    {
        _playerController = Player.Instance.GetComponent<PlayerController>();
        _playerController.SetAnimation("isRunning", true);
    }

    public void Exit()
    {
        _playerController.SetAnimation("isRunning", false);   
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
        else if (_playerController.magnitude <= 0.1f)
        {
            _playerController.SetState(new IdleState());
        }
    }
}
