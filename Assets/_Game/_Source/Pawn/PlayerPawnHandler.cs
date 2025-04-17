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

    [Inject]
    private void Construct()
    {
        _pawn.Construct();
        _pawn._moveAction = new MoveAction(_pawn, new BasePlayerMove());
        //_pawn._attackAction = new AttackAction();

        _pawn.OnTurnBegin += EnableInput;
        _pawn.OnTurnOver += DisableInput;

        _input = new PlayerInput();
        _input.GameplayInput.DoAction.performed += context => DoAction();
    }

    public void EnableInput()
    {
        Debug.Log("enabled");

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
        Debug.Log("Choose default");
        
        _tiles = _pawn._moveAction.CalculateTargets();
        GridManager.Instance.MarkTiles(_tiles, MarkerType.interact);

        //_tiles = _pawn._attackAction.CalculateTargets();
        // GridManager.Instance.MarkTiles(_tiles, MarkerType.attack);
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
            _pawn.BonusActions[typeof(MoveAction)].Perform(Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()), () => HandleActionEnding());
            //_pawn.Actions[typeof(AttackAction)].Perform(Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()), () => HandleActionEnding());
        }

    }

    private void HandleActionEnding()
    {
        _chosenAction = null;
        if (_pawn.PawnStats.CurrentAP <= 0)
        {
            _pawn.OnTurnOver.Invoke();
        } else
        {
            EnableInput();
        }
    }


    private void SkipTurn()
    {

    }
}
