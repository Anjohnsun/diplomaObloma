using UnityEngine;
using Zenject;

public class PlayerTurnState : IGameState
{
    private PlayerPawn _playerPawn;
    private GameplayUI _UI;
    public PlayerTurnState(PlayerPawn playerPawn, GameplayUI ui)
    {
        _playerPawn = playerPawn;
        _UI = ui;
    }

    public void Enter()
    {
        Debug.Log("-> Player Turn State");

        _UI.ShowGameplayInterface();
        _playerPawn.EnableInput(true);
    }

    public void Exit()
    {
        
    }
}
