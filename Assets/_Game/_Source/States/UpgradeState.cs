using UnityEngine;
using Zenject;

public class UpgradeState : IGameState
{
    private GameplayUI _gameplayUI;
    private PlayerPawnHandler _playerPawnHandler;

    public UpgradeState(GameplayUI gameplayUI, PlayerPawnHandler playerPawnHandler)
    {
        _gameplayUI = gameplayUI;
        _playerPawnHandler = playerPawnHandler;
    }
    public void Enter()
    {
        Debug.Log("-> Upgrade State");
        _gameplayUI.UnlockUpgrade(true);
        _playerPawnHandler._endlessActions = true;

        _playerPawnHandler.EnableInput();
    }

    public void Exit()
    {
        _gameplayUI.UnlockUpgrade(false);
        _playerPawnHandler._endlessActions = false;
    }
}
