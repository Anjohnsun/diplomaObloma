using System;
using UnityEngine;
using Zenject;

public class PlayerPawnHandler : MonoBehaviour
{
    private Pawn _pawn;
    private PlayerInput _input;

    private Vector2Int _clickPoint;
    private Vector2Int _hoverPoint;

    public Vector2Int ClickPoint => _clickPoint;
    public Vector2Int HoverPoint => _hoverPoint;

    private Action _onPlayerTurnStart;

    [Inject]
    private void Construct(Pawn pawn, [Inject()]bool fromSave)
    {
        _input = new PlayerInput();
        _pawn = pawn;

        //подписки инпута
        _input.GameplayInput.DoAction.performed += context => DoAction();


        //наполнение playerPawn декораторами и действиями
        if (fromSave)
        {
            //логика загрузка сохранённых данных
            // load from SaveService()...
        } else
        {
            
        }
    }

    public void EnableInput(bool value)
    {
        if (value)
            _input.Enable();
        else
            _input.Disable();

        _onPlayerTurnStart.Invoke();
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
