using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Zenject;

public class PlayerPawnHandler : MonoBehaviour
{
    [SerializeField] private Pawn _pawn;

    private PlayerInput _input;
    private bool _inputEnabled;

    private Type _chosenAction;
    List<FieldTile> _tiles;

    private GameplayUI _gameplayUI;
    public bool _endlessActions = false;

    public List<Button> _actionButtons;

    [Inject]
    private void Construct(GameplayUI gameplayUI)
    {
        _pawn.Construct(0, 1, 0, 0);
        _pawn.MoveAction = new MoveAction(_pawn, new BasePlayerMove());
        _pawn.AttackAction = new AttackAction(_pawn, new LittleSword(_pawn));

        _pawn.AddBonusAction(new HealAction(_pawn, 1));
        _pawn.AddBonusAction(new SwapAction(_pawn));
        _pawn.AddBonusAction(new MagicArrowAction(_pawn, 2));
        _pawn.AddBonusAction(new FireballAction(_pawn, 3, 1));

        _actionButtons[0].onClick.AddListener(() => ChooseAction<SwapAction>());
        _actionButtons[1].onClick.AddListener(() => ChooseAction<HealAction>());
        _actionButtons[2].onClick.AddListener(() => ChooseAction<MagicArrowAction>());
        _actionButtons[3].onClick.AddListener(() => ChooseAction<FireballAction>());

        _gameplayUI = gameplayUI;
        _gameplayUI.Costruct(_pawn.PawnStats);

        _gameplayUI.UpgradeButtons[0].onClick.AddListener(() => _pawn.PawnStats.LevelUpStat(StatType.AP));
        _gameplayUI.UpgradeButtons[1].onClick.AddListener(() => _pawn.PawnStats.LevelUpStat(StatType.HP));
        _gameplayUI.UpgradeButtons[2].onClick.AddListener(() => _pawn.PawnStats.LevelUpStat(StatType.ARM));
        _gameplayUI.UpgradeButtons[3].onClick.AddListener(() => _pawn.PawnStats.LevelUpStat(StatType.STR));

        _pawn.PawnStats.OnStatsChanged += UpdateUI;
        UpdateUI(_pawn.PawnStats.CurrentHP, _pawn.PawnStats.CurrentAP, _pawn.PawnStats.STR, _pawn.PawnStats.ARM);

        _input = new PlayerInput();
        _input.GameplayInput.DoAction.performed += context => DoAction();

    }
    public void EnableInput()
    {
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
        _gameplayUI.UpdatePlayerStats(_pawn.PawnStats);
    }
    private void ChooseAction<T>() where T : IPawnAction
    {
        Debug.Log($"Попытка выбрать {typeof(T).ToString()}");
        GridManager.Instance.DemarkTiles();
        if (_inputEnabled)
        {
            if (_chosenAction != typeof(T))
            {
                _chosenAction = typeof(T);
                _tiles = _pawn.BonusActions[typeof(T)].CalculateTargets();
                GridManager.Instance.MarkTiles(_tiles, MarkerType.interact);
            }
            else
            {
                _chosenAction = null;
                ChooseDefaultActions();
            }
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
        if (_endlessActions)
        {
            _pawn.PawnStats.ResetAP();
        }

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
