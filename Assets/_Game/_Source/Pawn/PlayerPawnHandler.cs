using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class PlayerPawnHandler : MonoBehaviour
{
    private Pawn _pawn;
    [SerializeField] private PawnStatsSO _pawnStats;
    private PlayerInput _input;
    private StateManager _stateManager;
    private EnemyTurnState _enemyTurnState;

    private Vector2Int _clickPoint;
    private Vector2Int _hoverPoint;

    public Vector2Int ClickPoint => _clickPoint;
    public Vector2Int HoverPoint => _hoverPoint;

    private GridManager _gridManager;

    private IPawnAction _chosenAction;


    [Inject]
    private void Construct([Inject(Id = "playerPawn")] Pawn pawn,/* [Inject(Id = "fromSave")]bool fromSave,*/ GridManager gridManager,
        StateManager stateManager, EnemyTurnState enemyTurnState)
    {
        _input = new PlayerInput();
        _pawn = pawn;
        _gridManager = gridManager;
        _stateManager = stateManager;
        _enemyTurnState = enemyTurnState;

        //подписки инпута
        _input.GameplayInput.DoAction.performed += context => DoAction();


        //наполнение playerPawn декораторами и действиями
        /*        if (fromSave)
                {
                    //логика загрузка сохранённых данных
                    // load from SaveService()...
                } else
                {*/
        _pawn.Construct(_pawnStats, _gridManager);
        _pawn.AddAction(new MoveAction(_pawn, new PlusMove(_gridManager), _gridManager));
        //add basic attack
        //maybe add start effects?
        //}
    }

    public void EnableInput(bool value)
    {
        if (value)
        {
            _input.Enable();

            //отражение действия по умолчанию
            ChooseAction<MoveAction>();
        }
        else
        {
            _input.Disable();
        }
    }

    public void ChooseAction<T>() where T : IPawnAction
    {
        _pawn.Actions.TryGetValue(typeof(T), out _chosenAction);
        var targets = _chosenAction.CalculateTargets();
        _gridManager.HighlightTileGroup(targets);
    }


    private void ChooseAction()
    {
        var targets = _chosenAction.CalculateTargets();
        _gridManager.HighlightTileGroup(targets);
    }

    private void DoAction()
    {
        _clickPoint = Vector2Int.RoundToInt(Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()));

        _chosenAction.Perform(_clickPoint);
        _gridManager.DehighlightAllTiles();

        StartCoroutine(HandleEndOfAction(_chosenAction.duration));
    }

    private IEnumerator HandleEndOfAction(float delay)
    {
        _input.Disable();
        yield return new WaitForSeconds(delay);

        if (_pawn.PawnStats.ActionPointsLeft <= 0)
        {
            Debug.Log("END OF TURN");
            _stateManager.ChangeState(_enemyTurnState);
        }
        else
        {
            ChooseAction();
        }
        _input.Enable();
    }

    private void SkipTurn()
    {
        while (_pawn.PawnStats.ActionPointsLeft > 0)
        {
            _pawn.PawnStats.UseAction();
        }
        StartCoroutine(HandleEndOfAction(0));
    }
}
