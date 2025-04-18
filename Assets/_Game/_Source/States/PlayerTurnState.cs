using UnityEngine;
using Zenject;

public class PlayerTurnState : IGameState
{
    private PlayerPawnHandler _playerHandler;
    public PlayerTurnState(PlayerPawnHandler playerHandler)
    {
        _playerHandler = playerHandler;
    }

    public void Enter()
    {
        Debug.Log("-> Player Turn State");

        _playerHandler.EnableInput();
    }

    public void Exit()
    {
        
    }
}
