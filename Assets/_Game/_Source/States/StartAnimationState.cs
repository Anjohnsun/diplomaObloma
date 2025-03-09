using System.Collections;
using UnityEngine;
using Zenject;

public class StartAnimationState : AGameState
{
    private  StateManager _gameManager;
    private  GridManager _gridManager;
    private IGameplayUIService _gameplayUIService;
    private  MonoBehaviour _coroutines;
    
    private StateManager _stateManager;
    private PlayerTurnState _playerTurnState;

    [Inject]
    public StartAnimationState(
        StateManager gameManager,
        GridManager gridManager,
        IGameplayUIService gameplayUIService,
        MonoBehaviour coroutines)
    {
        _gameManager = gameManager;
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
        // 1. ��������� ����
        _gridManager.CreateStartLines();

        // 2. ��������� UI
        _gameplayUIService.ShowGameplayInterface();

        // 3. ���������� ����� ������
        //Vector2Int playerStartPosition = new Vector2Int(0, 0); // ������ ��������� �������
        //_playerPawn.SetPosition(playerStartPosition);
        //yield return StartCoroutine(_playerPawn.PlaySpawnAnimation());

        // ������� � ���������� ��������� 
        _gameManager.ChangeState(_playerTurnState);

        return null;
    }

    public override void Exit()
    {
        Debug.Log("Exiting Start Animation State");
    }
}
