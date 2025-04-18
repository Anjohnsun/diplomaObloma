using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class PlayerPawnHandler : MonoBehaviour
{
    [SerializeField] private Pawn _pawn;
    private PlayerInput _input;
    private bool _inputEnabled;

    private Type _chosenAction;
    List<FieldTile> _tiles;

    [SerializeField] private TextMeshProUGUI _playerStatsUI;

    [Inject]
    private void Construct()
    {
        _pawn.Construct(0, 3, 0, 0);
        _pawn.MoveAction = new MoveAction(_pawn, new BasePlayerMove());
        _pawn.AttackAction = new AttackAction(_pawn, new LittleSword(_pawn));

        _pawn.PawnStats.OnStatsChanged += UpdateUI;
        UpdateUI(_pawn.PawnStats.CurrentHP, _pawn.PawnStats.CurrentAP, _pawn.PawnStats.STR, _pawn.PawnStats.ARM);

        _input = new PlayerInput();
        _input.GameplayInput.DoAction.performed += context => DoAction();
    }

    public void EnableInput()
    {
        Debug.Log("enabled");

        if (_pawn.PawnStats.CurrentAP <= 0)
        {
            _pawn.PawnStats.StartNewTurn();
        }

        _inputEnabled = true;
        _input.Enable();
        ChooseDefaultActions();
    }

    private void handleNewPawnAction()
    {

    }

    public void DisableInput()
    {
        _inputEnabled = false;
        _input.Disable();
    }

    private void ChooseDefaultActions()
    {
        GridManager.Instance.DemarkTiles();

        _tiles = _pawn.MoveAction.CalculateTargets();
        GridManager.Instance.MarkTiles(_tiles, MarkerType.interact);

        _tiles = _pawn.AttackAction.CalculateTargets();
        GridManager.Instance.MarkTiles(_tiles, MarkerType.attack);
    }

    private void UpdateUI(int hp, int ap, int str, int arm)
    {
        _playerStatsUI.text = $"HP - {hp}\n AP - {ap}\n STR - {str}\n ARM - {arm}";
    }

    private void ChooseAction<T>() where T : IPawnAction
    {
        GridManager.Instance.DemarkTiles();
        if (_inputEnabled)
        {
            if (_chosenAction != typeof(T))
            {
                _chosenAction = typeof(T);
                _tiles = _pawn.BonusActions[typeof(MoveAction)].CalculateTargets();
                GridManager.Instance.MarkTiles(_tiles, MarkerType.interact);
            }
            else
            {
                _chosenAction = null;
                ChooseDefaultActions();
            }
            /*GridManager.Instance.DemarkTiles();
            List<FieldTile> _tiles = _pawn.Actions[typeof(MoveAction)].CalculateTargets();
            GridManager.Instance.MarkTiles(_tiles, MarkerType.interact);*/
        }
    }

    private void DoAction()
    {
        DisableInput();
        GridManager.Instance.DemarkTiles();

        if (_chosenAction != null)
        {
            _pawn.BonusActions[_chosenAction].Perform(Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()), () => HandleActionEnding());
        }
        else
        {
            if (_pawn.MoveAction.CanPerform(Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue())))
                _pawn.MoveAction.Perform(Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()), () => HandleActionEnding());
            else if (_pawn.AttackAction.CanPerform(Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue())))
                _pawn.AttackAction.Perform(Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()), () => HandleActionEnding());
            else
            {
                HandleActionEnding();
            }
        }
    }

    private void HandleActionEnding()
    {
        _chosenAction = null;
        if (_pawn.PawnStats.CurrentAP <= 0)
        {
            if (_pawn.GridPosition.y == GridManager.Instance.VerticalSize - 1)
            {
                LevelManager.Instance.StartLevelTransition();
            }
            GridManager.Instance.DemarkTiles();
            StateManager.Instance.ChangeState<EnemyTurnState>();
        }
        else
        {
            if (_pawn.GridPosition.y == GridManager.Instance.VerticalSize - 1)
            {
                LevelManager.Instance.StartLevelTransition();
            }
            EnableInput();
        }
    }


    private void SkipTurn()
    {
        StateManager.Instance.ChangeState<EnemyTurnState>();
    }
}
