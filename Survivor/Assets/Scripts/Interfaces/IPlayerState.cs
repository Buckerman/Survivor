using Entities.Player;

public interface IPlayerState
{
    void Enter(PlayerController player);
    void Exit();
    void Update();
    void HandleInput();
}