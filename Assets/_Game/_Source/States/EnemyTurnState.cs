using UnityEngine;
using Zenject;

public class EnemyTurnState : IGameState
{
    private EnemyManager _enemyManager;
    private GameplayUI _gameplayUI;
    public EnemyTurnState(EnemyManager enemyManager, GameplayUI gameplayUI)
    {
        _enemyManager = enemyManager;
        _gameplayUI = gameplayUI;
    }

    public void Enter()
    {
        Debug.Log("-> Enemy Turn State");

        _enemyManager.StartEnemyTurn();
        if (_enemyManager.EnemyCount > 0)
            _gameplayUI.HideGameplayInterface();
    }

    public void Exit()
    {

    }
}
