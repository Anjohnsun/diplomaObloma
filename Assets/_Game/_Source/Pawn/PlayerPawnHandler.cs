using System;
using UnityEngine;
using Zenject;

public class PlayerPawnHandler : MonoBehaviour
{
    private Pawn _pawn;
    private PlayerInput _input;
    private StateManager _stateManager;
    private EnemyTurnState _enemyTurnState;

    private Vector2Int _clickPoint;
    private Vector2Int _hoverPoint;

    public Vector2Int ClickPoint => _clickPoint;
    public Vector2Int HoverPoint => _hoverPoint;

    private Action _onPlayerTurnStart;
    private GridManager _gridManager;

    private IPawnAction _chosenAction;


    [Inject]
    private void Construct(Pawn pawn, [Inject(Id = "fromSave")]bool fromSave, GridManager gridManager, StateManager stateManager)
    {
        _input = new PlayerInput();
        _pawn = pawn;
        _gridManager = gridManager;
        _stateManager = stateManager;

        //подписки инпута
        _input.GameplayInput.DoAction.performed += context => DoAction();


        //наполнение playerPawn декораторами и действиями
        if (fromSave)
        {
            //логика загрузка сохранённых данных
            // load from SaveService()...
        } else
        {
            _pawn.AddAction(new MoveAction(_pawn, new PlusMove(_gridManager), _gridManager));
            //add basic attack
            //maybe add start effects?
        }
    }

    public void EnableInput(bool value)
    {

        if (value)
            _input.Enable();
        else
            _input.Disable();

        _onPlayerTurnStart.Invoke();

        //отражение действия по умолчанию
        ChooseAction<MoveAction>();
    }

    public void ChooseAction<T>()  where T : IPawnAction
    {
        Debug.Log("ACTION CHOSEN");
        _pawn.Actions.TryGetValue(typeof(T), out _chosenAction);
        var targets = _chosenAction.CalculateTargets();
        _gridManager.HighlightTileGroup(targets, TileHighlightType.move);
    }

    private void DoAction()
    {
        //action choise logic

        //this should happen, when the action is finished!!!
        if(_pawn.PawnStats.ActionPointsLeft <= 0)
        {
            _stateManager.ChangeState(_enemyTurnState);
        }
    }

    private void SkipTurn()
    {
        //skip turn logic
    }

    public void PerformOnPlayerTurnStart(Action p)
    {
        _onPlayerTurnStart += p;
    }
}
