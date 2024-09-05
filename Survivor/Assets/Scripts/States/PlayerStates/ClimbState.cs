using UnityEngine;

public class ClimbingState : IPlayerState
{
    private PlayerController _playerController;

    public void Enter()
    {
        _playerController = Player.Instance.GetComponent<PlayerController>();
        _playerController.SetAnimation("isClimbing", true);
        _playerController.SetAnimation("isRunning", false);
        _playerController.SetAnimation("isJumping", false);
        _playerController.OnJumpAnimationEnd();
    }

    public void Exit()
    {
        _playerController.SetAnimation("isClimbing", false);
    }

    public void Update()
    {
        HandleInput();
    }

    public void HandleInput()
    {
        if (!_playerController.IsClimbing)
        {
            if (Mathf.Abs(Input.GetAxis("Horizontal")) > 0.1f || Mathf.Abs(Input.GetAxis("Vertical")) > 0.1f)
            {
                _playerController.SetState(new RunningState());
            }
            else _playerController.SetState(new IdleState());
        }
    }
}