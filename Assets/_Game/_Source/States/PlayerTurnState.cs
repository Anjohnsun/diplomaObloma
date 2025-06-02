using UnityEngine;
using Zenject;

public class PlayerTurnState : IGameState
{
    private PlayerPawn _playerPawn;
    public PlayerTurnState(PlayerPawn playerPawn)
    {
        _playerPawn = playerPawn;
    }

    public void Enter()
    {
        Debug.Log("-> Player Turn State");

        _playerPawn.EnableInput(true);
    }

    public void Exit()
    {
        
    }
}
