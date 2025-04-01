using UnityEngine;
using Zenject;

public class PlayerTurnState : AGameState
{
    private PlayerPawnHandler _playerPawnHandler;
    private IGameplayUIService _gameplayUIService;

    [Inject]
    public PlayerTurnState(PlayerPawnHandler playerPawnHandler, IGameplayUIService gameplayUIService)
    {
        Debug.LogWarning("PLAYER TURN STATE INJECTED");
        _playerPawnHandler = playerPawnHandler;
        _gameplayUIService = gameplayUIService;
    }

    public override void Enter()
    {
        Debug.Log("Player Turn State entered");
        _playerPawnHandler.EnableInput(true);
        _gameplayUIService.ShowGameplayInterface();
    }

    public override void Exit()
    {
        _playerPawnHandler.EnableInput(false);
        _gameplayUIService.HideGameplayInterface();
    }
}
