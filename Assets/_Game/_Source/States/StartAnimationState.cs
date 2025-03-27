using System.Collections;
using UnityEngine;
using Zenject;

public class StartAnimationState : AGameState
{
    private  StateManager _stateManager;
    private PlayerTurnState _playerTurnState;
    private  GridManager _gridManager;
    private IGameplayUIService _gameplayUIService;
    private  MonoBehaviour _coroutines;
  
    [Inject]
    public StartAnimationState(
        StateManager stateManager,
        GridManager gridManager,
        IGameplayUIService gameplayUIService,
        MonoBehaviour coroutines)
    {
        _stateManager = stateManager;
        _gridManager = gridManager;
        _gameplayUIService = gameplayUIService;
        _coroutines = coroutines;
    }

    public override void Enter()
    {
        Debug.Log("Entering Start Animation State");
        _coroutines.StartCoroutine(PlayStartAnimation());
    }

    private IEnumerator PlayStartAnimation()
    {
        // 1. Генерация поля
        _gridManager.CreateStartLines();
        _gridManager.SetPlayerPawnToStartPosition();

        // 2. Появление UI
        _gameplayUIService.ShowGameplayInterface();



        // Переход к следующему состоянию 
        _stateManager.ChangeState(_playerTurnState);

        return null;
    }

    public override void Exit()
    {
        Debug.Log("Exiting Start Animation State");
    }
}
