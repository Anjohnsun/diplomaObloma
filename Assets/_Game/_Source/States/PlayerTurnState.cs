using UnityEngine;
using Zenject;

public class PlayerTurnState : AGameState
{
    private PlayerPawnHandler _playerPawnHandler;

    [Inject]
    public PlayerTurnState(PlayerPawnHandler playerPawnHandler)
    {
        _playerPawnHandler = playerPawnHandler;
    }

    public override void Enter()
    {
        _playerPawnHandler.EnableInput(true);
    }

    public override void Exit()
    {
        _playerPawnHandler.EnableInput(false);
    }
}
