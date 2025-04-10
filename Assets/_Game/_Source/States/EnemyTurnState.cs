using UnityEngine;

public class EnemyTurnState : AGameState
{
    private EnemyManager _enemyManager;

    public override void Enter()
    {
        //запустить обработку хода в enemyManager
        base.Enter();
    }
}
