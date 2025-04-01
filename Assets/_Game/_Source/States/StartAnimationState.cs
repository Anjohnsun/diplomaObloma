using System.Collections;
using UnityEngine;
using Zenject;

public class StartAnimationState : AGameState
{
    private  StateManager _stateManager;
    private PlayerTurnState _playerTurnState;
    private  GridManager _gridManager;
    private  MonoBehaviour _coroutines;
  
    [Inject]
    public StartAnimationState(
        StateManager stateManager,
        PlayerTurnState playerTurnState,
        GridManager gridManager,
        MonoBehaviour coroutines)
    {
        _stateManager = stateManager;
        _playerTurnState = playerTurnState;
        _gridManager = gridManager;
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

        yield return new WaitForSeconds(3f);
        // Переход к следующему состоянию 
        _stateManager.ChangeState(_playerTurnState);

        yield return null;
    }

    public override void Exit()
    {
    }
}
