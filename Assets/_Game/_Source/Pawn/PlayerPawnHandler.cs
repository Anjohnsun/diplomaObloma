using System;
using UnityEngine;

public class PlayerPawnHandler : MonoBehaviour
{
    private Pawn _pawn;
    private PlayerInput _input;

    private Vector2Int _clickPoint;
    private Vector2Int _hoverPoint;

    public Vector2Int ClickPoint => _clickPoint;
    public Vector2Int HoverPoint => _hoverPoint;

    private Action _onPlayerTurnStart;

    private void Construct(Pawn pawn)
    {
        _input = new PlayerInput();
        _pawn = pawn;

        //подписки инпута
        _input.GameplayInput.DoAction.performed += context => DoAction();

        //наполнение playerPawn декораторами и действиями
    }

    public void EnableInput(bool value)
    {
        if (value)
            _input.Enable();
        else
            _input.Disable();
    }

    private void DoAction()
    {
        //action choise logic
    }

    public void PerformOnPlayerTurnStart(Action p)
    {
        _onPlayerTurnStart += p;
    }
}
