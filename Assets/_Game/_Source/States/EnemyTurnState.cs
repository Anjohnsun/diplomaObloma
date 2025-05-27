using UnityEngine;
using Zenject;

public class EnemyTurnState : IGameState
{
    private EnemyManager _enemyManager;
    public EnemyTurnState(EnemyManager enemyManager)
    {
        _enemyManager = enemyManager;
    }

    public void Enter()
    {
        _enemyManager.StartEnemyTurn();
    }

    public void Exit()
    {

    }
}
