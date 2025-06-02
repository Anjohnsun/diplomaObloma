using UnityEngine;
using Zenject;

public class UpgradeState : IGameState
{
    private GameplayUI _gameplayUI;
    private PlayerPawn _playerPawn;

    public UpgradeState(GameplayUI gameplayUI, PlayerPawn playerPawn)
    {
        _gameplayUI = gameplayUI;
        _playerPawn = playerPawn;
    }
    public void Enter()
    {
        Debug.Log("-> Upgrade State");

        _gameplayUI.UnlockUpgrade(true);
        _playerPawn.UnlockUpgrade(true);

        _playerPawn.EnableInput(true);
    }

    public void Exit()
    {
        _gameplayUI.UnlockUpgrade(false);
        _playerPawn.UnlockUpgrade(false);
    }
}
